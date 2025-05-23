﻿using System.Text.Json;
using backend.Product.Controllers.Product;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.InfrastructureLayer.Cache;

public class CreateProductCache(IDatabase redisDatabase) : ICreateProductCache
{
    public async Task<CachedItem<CreateProductResponse>?> GetAsync(string key)
    {
        RedisValue cachedValue = await redisDatabase.StringGetAsync(key);

        return cachedValue.IsNull ? null : JsonSerializer.Deserialize<CachedItem<CreateProductResponse>>(cachedValue!);
    }

    public async Task SetAsync(string key, CachedItem<CreateProductResponse> value, TimeSpan expiry)
    {
        string serializedValue = JsonSerializer.Serialize(value);
        await redisDatabase.StringSetAsync(key, serializedValue, expiry);
    }
}
