// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Cache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.Tests.TestHelpers.Fakes.ListProductsCacheFake;

public class ListProductsCacheFake : IListProductsCache
{
    private readonly Dictionary<string, CachedItem<ListProductsResponse>> _cache = new();
    private readonly Dictionary<string, Dictionary<string, long>> _hashData = new();

    public Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        CachedItem<ListProductsResponse> entry = value with { Timestamp = DateTime.UtcNow.Add(expiry) };

        _cache[key] = entry;
        return Task.CompletedTask;
    }

    public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
    {
        if (!_hashData.TryGetValue(key!, out Dictionary<string, long>? hash))
        {
            return Task.FromResult(Array.Empty<HashEntry>());
        }

        var entries = hash.Select(kv => new HashEntry(kv.Key, kv.Value)).ToArray();
        return Task.FromResult(entries);
    }

    public Task<long> HashIncrementAsync(string key, string field)
    {
        if (!_hashData.TryGetValue(key, out Dictionary<string, long>? hash))
        {
            hash = new Dictionary<string, long>();
            _hashData[key] = hash;
        }

        long currentValue = hash.GetValueOrDefault(field, 0);

        long newValue = currentValue + 1;
        hash[field] = newValue;

        return Task.FromResult(newValue);
    }

    public Task<BatchResult> ExecuteBatchAsync(Action<IBatchOperations> batchAction)
    {
        var operations = new ListProductsBatchOperationsFake(this);
        var result = new BatchResult();

        batchAction(operations);

        result.HashIncrements = operations.HashIncrements;

        return Task.FromResult(result);
    }

    public Task<CachedItem<ListProductsResponse>?> GetAsync(string key)
    {
        if (!_cache.TryGetValue(key, out CachedItem<ListProductsResponse>? entry))
        {
            return Task.FromResult<CachedItem<ListProductsResponse>?>(null);
        }

        if (entry.Timestamp < DateTime.UtcNow)
        {
            _cache.Remove(key);
            return Task.FromResult<CachedItem<ListProductsResponse>?>(null);
        }

        return Task.FromResult(entry)!;
    }
}
