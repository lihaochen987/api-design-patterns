// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes.CreateProductCacheFake;

public class CreateProductExceptionThrowingCacheFake : ICreateProductCache
{
    public readonly HashSet<string> KeysToThrowOn = [];

    public void SetKeyToThrowOn(string key)
    {
        KeysToThrowOn.Add(key);
    }

    public Task SetAsync(string key, CachedItem<CreateProductResponse> value, TimeSpan expiry)
    {
        if (KeysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.CompletedTask;
    }

    public async Task<CachedItem<CreateProductResponse>?> GetAsync(string key)
    {
        if (KeysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return await Task.FromResult<CachedItem<CreateProductResponse>?>(null);
    }
}
