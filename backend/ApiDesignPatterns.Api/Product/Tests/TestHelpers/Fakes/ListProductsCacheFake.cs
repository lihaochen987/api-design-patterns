// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;
using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ListProductsCacheFake : ICache<CachedItem<ListProductsResponse>>
{
    private readonly Dictionary<string, CachedItem<ListProductsResponse>> _cache = new();

    public Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        var entry = new CachedItem<ListProductsResponse> { Item = value.Item, Timestamp = DateTime.UtcNow.Add(expiry) };

        _cache[key] = entry;
        return Task.CompletedTask;
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
