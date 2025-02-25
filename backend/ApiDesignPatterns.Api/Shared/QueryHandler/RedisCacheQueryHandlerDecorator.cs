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
        string cacheKey = GenerateCacheKey(query);

        try
        {
            RedisValue cached = await redisCache.StringGetAsync(cacheKey);

            if (!cached.HasValue)
            {
                var result = await queryHandler.Handle(query);

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
                    long totalChecks = stats.FirstOrDefault(x => x.Name == "total_checks").Value.TryParse(out long t)
                        ? t
                        : 0;
                    long staleHits = stats.FirstOrDefault(x => x.Name == "stale_hits").Value.TryParse(out long s)
                        ? s
                        : 0;

                    if (totalChecks >= 100) // Only start warning after we have enough samples
                    {
                        LogStalenessStatistics(
                            typeof(TQuery).Name,
                            new StalenessRates(
                                stalenessOptions.MinAcceptableRate,
                                stalenessOptions.MaxAcceptableRate,
                                (double)staleHits / totalChecks),
                            stalenessOptions.Ttl,
                            logger);
                    }

                    try
                    {
                        string serialized = JsonSerializer.Serialize(freshResult);
                        TimeSpan ttlWithJitter = JitterUtility.AddJitter(stalenessOptions.Ttl);
                        await redisCache.StringSetAsync(cacheKey, serialized, ttlWithJitter);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error caching query result");
                    }
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

    private record StalenessRates(
        double MinAcceptableRate,
        double MaxAcceptableRate,
        double Rate)
    {
        public double Percentage => Rate * 100;
    }

    private static void LogStalenessStatistics(
        string queryTypeName,
        StalenessRates stalenessRates,
        TimeSpan ttl,
        ILogger logger)
    {
        if (stalenessRates.Rate > stalenessRates.MaxAcceptableRate)
        {
            LogHighStalenessRate(queryTypeName, stalenessRates, ttl, logger);
        }
        else if (stalenessRates.Rate < stalenessRates.MinAcceptableRate)
        {
            LogLowStalenessRate(queryTypeName, stalenessRates, ttl, logger);
        }
        else
        {
            LogAcceptableStalenessRate(queryTypeName, stalenessRates, logger);
        }
    }

    private static void LogHighStalenessRate(
        string queryTypeName,
        StalenessRates stalenessRates,
        TimeSpan ttl,
        ILogger logger)
    {
        logger.LogWarning(
            "High stale rate detected for {QueryType}. Current rate: {StaleRate}% exceeds maximum acceptable {MaxRate}%. " +
            "Consider decreasing TTL by 50% (current TTL: {CurrentTTL})",
            queryTypeName,
            stalenessRates.Percentage.ToString("F2"),
            (stalenessRates.MaxAcceptableRate * 100).ToString("F2"),
            ttl.TotalMinutes);
    }

    private static void LogLowStalenessRate(
        string queryTypeName,
        StalenessRates stalenessRates,
        TimeSpan ttl,
        ILogger logger)
    {
        logger.LogWarning(
            "Low stale rate detected for {QueryType}. Current rate: {StaleRate}% below minimum threshold {MinRate}%. " +
            "Consider increasing TTL by 25% (current TTL: {CurrentTTL})",
            queryTypeName,
            stalenessRates.Percentage.ToString("F2"),
            (stalenessRates.MinAcceptableRate * 100).ToString("F2"),
            ttl.TotalMinutes);
    }

    private static void LogAcceptableStalenessRate(
        string queryTypeName,
        StalenessRates stalenessRates,
        ILogger logger)
    {
        logger.LogInformation(
            "Stale data detected for {QueryType}. Current rate: {StaleRate}% (within acceptable range of {MinRate}% - {MaxRate}%)",
            queryTypeName,
            stalenessRates.Percentage.ToString("F2"),
            (stalenessRates.MinAcceptableRate * 100).ToString("F2"),
            (stalenessRates.MaxAcceptableRate * 100).ToString("F2"));
    }

    private static string GenerateCacheKey(TQuery query)
    {
        string queryType = typeof(TQuery).Name;
        string queryHash = JsonSerializer.Serialize(query);
        return $"query:{queryType}:{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(queryHash))}";
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
