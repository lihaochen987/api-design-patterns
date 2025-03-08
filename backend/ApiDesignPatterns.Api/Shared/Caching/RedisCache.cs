// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using StackExchange.Redis;

namespace backend.Shared.Caching;

public class RedisCache(IDatabase redisDatabase) : ICache
{
    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        RedisValue cachedValue = await redisDatabase.StringGetAsync(key);

        if (cachedValue.IsNull)
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(cachedValue!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
    {
        string serializedValue = JsonSerializer.Serialize(value);
        await redisDatabase.StringSetAsync(key, serializedValue, expiry);
    }
}
