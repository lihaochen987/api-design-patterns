// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.InfrastructureLayer.Cache;

public interface IListProductsCache
{
    Task<CachedItem<ListProductsResponse>?> GetAsync(string key);
    Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry);
    IBatch CreateBatch();
    Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None);
}
