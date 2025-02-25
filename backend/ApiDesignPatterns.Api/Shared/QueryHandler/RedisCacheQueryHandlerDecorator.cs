// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Shared.QueryHandler;

public class RedisCacheQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> queryHandler,
    IDatabase redisCache,
    ILogger<RedisCacheQueryHandlerDecorator<TQuery, TResult>> logger,
    CacheStalenessOptions stalenessOptions)
    : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    private static string StatsKey => $"cache:stats:{typeof(TQuery).Name}";

    public async Task<TResult?> Handle(TQuery query)
    {
        Random random = new();
        string queryType = typeof(TQuery).Name;
        string queryHash = JsonSerializer.Serialize(query);
        string cacheKey = $"query:{queryType}:{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(queryHash))}";

        try
        {
            RedisValue cached = await redisCache.StringGetAsync(cacheKey);

            if (!cached.HasValue)
            {
                return await SetValue(query, cacheKey);
            }

            var cachedResult = JsonSerializer.Deserialize<TResult>(cached!);

            // Randomly sample to check for staleness
            if (!(random.NextDouble() < stalenessOptions.CheckRate))
            {
                return cachedResult;
            }

            try
            {
                var freshResult = await queryHandler.Handle(query);

                var batch = redisCache.CreateBatch();
                await batch.HashIncrementAsync(StatsKey, "total_checks");

                if (!ResultsEqual(cachedResult, freshResult))
                {
                    await batch.HashIncrementAsync(StatsKey, "stale_hits");

                    // Get current stats and calculate rate
                    HashEntry[] stats = await redisCache.HashGetAllAsync(StatsKey);
                    long total = stats.FirstOrDefault(x => x.Name == "total_checks").Value.TryParse(out long t)
                        ? t
                        : 0;
                    long stale = stats.FirstOrDefault(x => x.Name == "stale_hits").Value.TryParse(out long s)
                        ? s
                        : 0;

                    if (total >= 100) // Only start warning after we have enough samples
                    {
                        double staleRate = (double)stale / total;
                        double stalePercentage = staleRate * 100;

                        if (staleRate > stalenessOptions.MaxAcceptableRate)
                        {
                            logger.LogWarning(
                                "High stale rate detected for {QueryType}. Current rate: {StaleRate}% exceeds maximum acceptable {MaxRate}%. " +
                                "Consider decreasing TTL by 50% (current TTL: {CurrentTTL})",
                                typeof(TQuery).Name,
                                stalePercentage.ToString("F2"),
                                (stalenessOptions.MaxAcceptableRate * 100).ToString("F2"),
                                stalenessOptions.Ttl.TotalMinutes);
                        }
                        else if (staleRate < stalenessOptions.MinAcceptableRate)
                        {
                            logger.LogWarning(
                                "Low stale rate detected for {QueryType}. Current rate: {StaleRate}% below minimum threshold {MinRate}%. " +
                                "Consider increasing TTL by 25% (current TTL: {CurrentTTL})",
                                typeof(TQuery).Name,
                                stalePercentage.ToString("F2"),
                                (stalenessOptions.MinAcceptableRate * 100).ToString("F2"),
                                stalenessOptions.Ttl.TotalMinutes);
                        }
                        else
                        {
                            logger.LogInformation(
                                "Stale data detected for {QueryType}. Current rate: {StaleRate}% (within acceptable range of {MinRate}% - {MaxRate}%)",
                                typeof(TQuery).Name,
                                stalePercentage.ToString("F2"),
                                (stalenessOptions.MinAcceptableRate * 100).ToString("F2"),
                                (stalenessOptions.MaxAcceptableRate * 100).ToString("F2"));
                        }
                    }

                    await SetValue(query, cacheKey, freshResult);
                }

                batch.Execute();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking for stale data");
            }

            return cachedResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error accessing cache for query {QueryType}", typeof(TQuery).Name);
            return await queryHandler.Handle(query);
        }
    }

    private async Task<TResult?> SetValue(TQuery query, string cacheKey, TResult? result = default)
    {
        result ??= await queryHandler.Handle(query);

        try
        {
            string serialized = JsonSerializer.Serialize(result);
            TimeSpan ttlWithJitter = JitterUtility.AddJitter(stalenessOptions.Ttl);
            await redisCache.StringSetAsync(cacheKey, serialized, ttlWithJitter);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error caching query result");
        }

        return result;
    }

    private static bool ResultsEqual(TResult? a, TResult? b)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;

        string jsonA = JsonSerializer.Serialize(a);
        string jsonB = JsonSerializer.Serialize(b);
        return jsonA == jsonB;
    }
}
