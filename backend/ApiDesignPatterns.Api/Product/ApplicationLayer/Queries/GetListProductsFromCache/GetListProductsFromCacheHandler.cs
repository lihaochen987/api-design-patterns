// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.QueryHandler;
using StackExchange.Redis;

namespace backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;

public class GetListProductsFromCacheHandler(IDatabase redisCache) : IQueryHandler<GetListProductsFromCacheQuery, ListProductsResponse>
{
    public async Task<ListProductsResponse?> Handle(GetListProductsFromCacheQuery query)
    {
        try
        {
            RedisValue cachedValue = await redisCache.StringGetAsync(query.CacheKey);

            if (cachedValue.IsNull)
            {
                return null;
            }

            var cachedData = JsonSerializer.Deserialize<CachedItem<ListProductsResponse>>(cachedValue!);
            return cachedData?.Item;
        }
        catch
        {
            return null;
        }
    }
}
