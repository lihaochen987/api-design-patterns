// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes.BatchCreateProductCacheFake;

public class BatchCreateProductCacheFake : IBatchCreateProductsCache
{
    private readonly Dictionary<string, CachedItem<IEnumerable<CreateProductResponse>>> _cache = new();

    public async Task<CachedItem<IEnumerable<CreateProductResponse>>?> GetAsync(string key)
    {
        if (!_cache.TryGetValue(key, out var entry))
        {
            return await Task.FromResult<CachedItem<IEnumerable<CreateProductResponse>>?>(null);
        }

        if (entry.Timestamp < DateTime.UtcNow)
        {
            _cache.Remove(key);
            return await Task.FromResult<CachedItem<IEnumerable<CreateProductResponse>>?>(null);
        }

        return await Task.FromResult(entry);
    }

    public Task SetAsync(string key, CachedItem<IEnumerable<CreateProductResponse>> value, TimeSpan expiry)
    {
        var entry = value with { Timestamp = DateTime.UtcNow.Add(expiry) };

        _cache[key] = entry;
        return Task.CompletedTask;
    }
}
