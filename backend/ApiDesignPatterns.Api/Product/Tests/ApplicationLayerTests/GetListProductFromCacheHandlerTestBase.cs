// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class GetListProductsFromCacheHandlerTestBase
{
    protected readonly RedisCacheFake Cache = new();
    protected readonly ExceptionThrowingCache ThrowingCache = new();
    protected readonly Fixture Fixture = new();

    protected IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> GetListProductsFromCacheHandler()
    {
        return new GetListProductsFromCacheHandler(Cache);
    }

    protected IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> GetExceptionThrowingHandler()
    {
        return new GetListProductsFromCacheHandler(ThrowingCache);
    }

    protected async Task SetupCacheWithData(string cacheKey, ListProductsResponse response)
    {
        var cachedItem = new CachedItem<ListProductsResponse> { Item = response };
        await Cache.SetAsync(cacheKey, cachedItem);
    }

    protected void SetupCacheWithNoData(string cacheKey)
    {
        Cache.Clear();
    }

    protected void SetupCacheToThrowException(string cacheKey)
    {
        ThrowingCache.SetKeyToThrowOn(cacheKey);
    }
}
