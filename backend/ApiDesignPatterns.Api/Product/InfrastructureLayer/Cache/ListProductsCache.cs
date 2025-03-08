// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.InfrastructureLayer.Cache;

public class ListProductsCache(IDatabase redisDatabase) : ICache<CachedItem<ListProductsResponse>>
{
    public async Task<CachedItem<ListProductsResponse>?> GetAsync(string key)
    {
        RedisValue cachedValue = await redisDatabase.StringGetAsync(key);

        return cachedValue.IsNull ? null : JsonSerializer.Deserialize<CachedItem<ListProductsResponse>>(cachedValue!);
    }

    public async Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        string serializedValue = JsonSerializer.Serialize(value);
        await redisDatabase.StringSetAsync(key, serializedValue, expiry);
    }

    public IBatch CreateBatch()
    {
        return redisDatabase.CreateBatch();
    }
}
