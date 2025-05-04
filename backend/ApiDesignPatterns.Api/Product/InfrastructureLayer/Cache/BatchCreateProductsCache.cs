// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Product.Controllers.Product;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.InfrastructureLayer.Cache;

public class BatchCreateProductsCache(IDatabase redisDatabase) : IBatchCreateProductsCache
{
    public async Task<CachedItem<IEnumerable<CreateProductResponse>>?> GetAsync(string key)
    {
        RedisValue cachedValue = await redisDatabase.StringGetAsync(key);

        return cachedValue.IsNull
            ? null
            : JsonSerializer.Deserialize<CachedItem<IEnumerable<CreateProductResponse>>>(cachedValue!);
    }

    public async Task SetAsync(string key, CachedItem<IEnumerable<CreateProductResponse>> value, TimeSpan expiry)
    {
        string serializedValue = JsonSerializer.Serialize(value);
        await redisDatabase.StringSetAsync(key, serializedValue, expiry);
    }
}
