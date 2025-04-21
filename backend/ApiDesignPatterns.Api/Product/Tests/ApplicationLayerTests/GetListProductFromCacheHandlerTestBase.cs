// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Product.Tests.TestHelpers.Fakes.ListProductsCacheFake;
using backend.Product.Tests.TestHelpers.Fakes.ListProductsExceptionThrowingCacheFake;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class GetListProductsFromCacheHandlerTestBase
{
    protected readonly ListProductsCacheFake Cache = new();
    protected readonly ListProductsExceptionThrowingCacheFake ThrowingCache = new();
    protected readonly Fixture Fixture = new();

    protected IAsyncQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> GetListProductsFromCacheHandler()
    {
        return new GetListProductsFromCacheHandler(Cache);
    }

    protected IAsyncQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> GetExceptionThrowingHandler()
    {
        return new GetListProductsFromCacheHandler(ThrowingCache);
    }
}
