// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Product.InfrastructureLayer.Cache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using StackExchange.Redis;

namespace backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;

public class UpdateListProductStalenessHandler(
    IListProductsCache cache,
    ILogger<UpdateListProductStalenessHandler> logger)
    : ICommandHandler<UpdateListProductStalenessCommand>
{
    private static string StatsKey => $"cache:stats:{nameof(UpdateListProductStalenessHandler)}";

    public async Task Handle(UpdateListProductStalenessCommand command)
    {
        var batch = cache.CreateBatch();
        var totalChecksTask = batch.HashIncrementAsync(StatsKey, "total_checks");

        Task? staleHitsTask = null;
        if (!ResultsEqual(command.CachedResult, command.FreshResult))
        {
            await batch.HashIncrementAsync(StatsKey, "stale_hits");
            await GetCacheStatistics(cache, command.StalenessOptions, logger);
        }

        batch.Execute();

        await totalChecksTask;
        if (staleHitsTask != null)
            await staleHitsTask;
    }

    private static async Task GetCacheStatistics(
        IListProductsCache redisCache,
        CacheStalenessOptions options,
        ILogger<UpdateListProductStalenessHandler> statsLogger)
    {
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
                nameof(UpdateListProductStalenessHandler),
                new StalenessRates(
                    options.MinAcceptableRate,
                    options.MaxAcceptableRate,
                    (double)staleHits / totalChecks),
                options.Ttl,
                statsLogger);
        }
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

    private record StalenessRates(
        double MinAcceptableRate,
        double MaxAcceptableRate,
        double Rate)
    {
        public double Percentage => Rate * 100;
    }

    private static bool ResultsEqual(ListProductsResponse? a, ListProductsResponse? b)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;

        string jsonA = JsonSerializer.Serialize(a);
        string jsonB = JsonSerializer.Serialize(b);
        return jsonA == jsonB;
    }
}
