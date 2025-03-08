// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Shared.Caching;

public class ExceptionThrowingCache : ICache
{
    private readonly HashSet<string> _keysToThrowOn = [];

    public void SetKeyToThrowOn(string key)
    {
        _keysToThrowOn.Add(key);
    }

    public Task<T?> GetAsync<T>(string key) where T : class
    {
        if (_keysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.FromResult<T?>(null);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
    {
        if (_keysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.CompletedTask;
    }
}
