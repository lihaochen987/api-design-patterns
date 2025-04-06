// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Cache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ListProductsCacheFake : IListProductsCache
{
    private readonly Dictionary<string, CachedItem<ListProductsResponse>> _cache = new();

    public Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        CachedItem<ListProductsResponse> entry = value with { Timestamp = DateTime.UtcNow.Add(expiry) };

        _cache[key] = entry;
        return Task.CompletedTask;
    }

    public IBatch CreateBatch() => throw new NotImplementedException();
    public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None) => throw new NotImplementedException();

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
