// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Product.ProductControllers;
using backend.Shared;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.InfrastructureLayer.Cache;

public class ListProductsCache(IDatabase redisDatabase) : ICache<CachedItem<ListProductsResponse>>
{
    public async Task<CachedItem<ListProductsResponse>?> GetAsync(string key)
    {
        RedisValue cachedValue = await redisDatabase.StringGetAsync(key);

        if (cachedValue.IsNull)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CachedItem<ListProductsResponse>>(cachedValue!);
    }

    public async Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        string serializedValue = JsonSerializer.Serialize(value);
        TimeSpan expiryWithJitter = JitterUtility.AddJitter(expiry);
        await redisDatabase.StringSetAsync(key, serializedValue, expiryWithJitter);
    }
}
