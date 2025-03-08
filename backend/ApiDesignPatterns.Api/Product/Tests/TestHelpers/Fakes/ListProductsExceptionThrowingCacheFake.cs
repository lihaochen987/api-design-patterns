// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;
using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes;

public class ListProductsExceptionThrowingCacheFake : ICache<CachedItem<ListProductsResponse>>
{
    private readonly HashSet<string> _keysToThrowOn = [];

    public void SetKeyToThrowOn(string key)
    {
        _keysToThrowOn.Add(key);
    }

    public Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        if (_keysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.CompletedTask;
    }

    public Task<CachedItem<ListProductsResponse>?> GetAsync(string key)
    {
        if (_keysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.FromResult<CachedItem<ListProductsResponse>?>(null);
    }
}
