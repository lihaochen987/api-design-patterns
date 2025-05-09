// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes.CreateProductCacheFake;

public class CreateProductCacheFake : ICreateProductCache
{
    private readonly Dictionary<string, CachedItem<CreateProductResponse>> _cache = new();

    public Task SetAsync(string key, CachedItem<CreateProductResponse> value, TimeSpan expiry)
    {
        CachedItem<CreateProductResponse> entry = value with { Timestamp = DateTime.UtcNow.Add(expiry) };

        _cache[key] = entry;
        return Task.CompletedTask;
    }

    public async Task<CachedItem<CreateProductResponse>?> GetAsync(string key)
    {
        if (!_cache.TryGetValue(key, out CachedItem<CreateProductResponse>? entry))
        {
            return await Task.FromResult<CachedItem<CreateProductResponse>?>(null);
        }

        if (entry.Timestamp < DateTime.UtcNow)
        {
            _cache.Remove(key);
            return await Task.FromResult<CachedItem<CreateProductResponse>?>(null);
        }

        return await Task.FromResult(entry);
    }
}
