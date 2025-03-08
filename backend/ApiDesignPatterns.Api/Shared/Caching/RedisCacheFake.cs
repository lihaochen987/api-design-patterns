// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;

namespace backend.Shared.Caching;

public class RedisCacheFake : ICache
{
    private readonly Dictionary<string, CacheEntry> _cache = new();

    public Task<T?> GetAsync<T>(string key) where T : class
    {
        if (!_cache.TryGetValue(key, out var entry))
        {
            return Task.FromResult<T?>(null);
        }

        if (entry.ExpiryTime.HasValue && entry.ExpiryTime < DateTime.UtcNow)
        {
            _cache.Remove(key);
            return Task.FromResult<T?>(null);
        }

        return Task.FromResult(JsonSerializer.Deserialize<T>(entry.SerializedValue)!)!;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
    {
        var entry = new CacheEntry
        {
            SerializedValue = JsonSerializer.Serialize(value),
            ExpiryTime = expiry.HasValue ? DateTime.UtcNow.Add(expiry.Value) : null
        };

        _cache[key] = entry;
        return Task.CompletedTask;
    }

    // Helper methods for testing
    public void Clear()
    {
        _cache.Clear();
    }

    public bool HasKey(string key)
    {
        return _cache.ContainsKey(key);
    }

    private class CacheEntry
    {
        public required string SerializedValue { get; init; }
        public DateTime? ExpiryTime { get; init; }
    }
}
