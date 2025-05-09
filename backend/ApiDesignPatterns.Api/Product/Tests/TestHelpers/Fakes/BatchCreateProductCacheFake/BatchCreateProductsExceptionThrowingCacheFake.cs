// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes.BatchCreateProductCacheFake;

public class BatchCreateProductsExceptionThrowingCacheFake : IBatchCreateProductsCache
{
    public readonly HashSet<string> KeysToThrowOn = [];

    public void SetKeyToThrowOn(string key)
    {
        KeysToThrowOn.Add(key);
    }

    public async Task<CachedItem<IEnumerable<CreateProductResponse>>?> GetAsync(string key)
    {
        if (KeysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return await Task.FromResult<CachedItem<IEnumerable<CreateProductResponse>>?>(null);
    }

    public Task SetAsync(string key, CachedItem<IEnumerable<CreateProductResponse>> value, TimeSpan expiry)
    {
        if (KeysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.CompletedTask;
    }
}
