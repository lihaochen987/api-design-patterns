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

        // Try to get from cache
        RedisValue cached;
        try
        {
            cached = await redisCache.StringGetAsync(cacheKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error accessing cache for query {QueryType}", typeof(TQuery).Name);
            return await queryHandler.Handle(query);
        }

        // Cache miss - get fresh result and cache it
        if (!cached.HasValue)
        {
            var result = await queryHandler.Handle(query);
            await CacheResult(result, cacheKey, stalenessOptions.Ttl);
            return result;
        }

        var cachedResult = JsonSerializer.Deserialize<TResult>(cached!);

        // Not checking for staleness - return cached result immediately
        if (random.NextDouble() >= stalenessOptions.CheckRate)
        {
            return cachedResult;
        }

        // Check for staleness
        var freshResult = await queryHandler.Handle(query);
        var batch = redisCache.CreateBatch();
        await batch.HashIncrementAsync(StatsKey, "total_checks");

        if (!ResultsEqual(cachedResult, freshResult))
        {
            await batch.HashIncrementAsync(StatsKey, "stale_hits");
            await GetCacheStatistics(redisCache, stalenessOptions, logger);
            await CacheResult(freshResult, cacheKey, stalenessOptions.Ttl);
        }

        batch.Execute();

        return cachedResult;
    }

    private static async Task GetCacheStatistics(
        IDatabase cache,
        CacheStalenessOptions options,
        ILogger<RedisCacheQueryHandlerDecorator<TQuery, TResult>> statsLogger)
    {
        // Get current stats and calculate rate
        HashEntry[] stats = await cache.HashGetAllAsync(StatsKey);
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
                    options.MinAcceptableRate,
                    options.MaxAcceptableRate,
                    (double)staleHits / totalChecks),
                options.Ttl,
                statsLogger);
        }
    }

    private async Task CacheResult(TResult? result, string cacheKey, TimeSpan ttl)
    {
        string serialized = JsonSerializer.Serialize(result);
        TimeSpan ttlWithJitter = JitterUtility.AddJitter(ttl);
        await redisCache.StringSetAsync(cacheKey, serialized, ttlWithJitter);
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
