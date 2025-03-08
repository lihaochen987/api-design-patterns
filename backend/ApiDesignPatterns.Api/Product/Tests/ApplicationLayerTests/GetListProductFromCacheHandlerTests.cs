// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class GetListProductsFromCacheHandlerTests : GetListProductsFromCacheHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheHasData()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5};
        var expectedResults = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(10), NextPageToken = "11"
        };
        var expectedResponse = new CachedItem<ListProductsResponse>
        {
            Item = expectedResults, Timestamp = DateTime.UtcNow + TimeSpan.FromMinutes(10)
        };
        await Cache.SetAsync("products:maxsize:10", expectedResponse, TimeSpan.FromMinutes(10));
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ProductsResponse.ShouldNotBeNull();
        result.ProductsResponse.Results.Count().ShouldBe(10);
        result.ProductsResponse.Results.ShouldBeEquivalentTo(expectedResponse.Item.Results);
        result.CacheKey.ShouldBe("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldReturnNullProductsResponse_WhenCacheDoesNotHaveData()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5};
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ProductsResponse.ShouldBeNull();
        result.CacheKey.ShouldBe("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldReturnNullProductsResponse_WhenCacheThrowsException()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5};
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetExceptionThrowingHandler();
        ThrowingCache.SetKeyToThrowOn("products:maxsize:10");

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ProductsResponse.ShouldBeNull();
        result.CacheKey.ShouldBe("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithPageToken()
    {
        var request = new ListProductsRequest { MaxPageSize = 10, PageToken = "token123" };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5};
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.CacheKey.ShouldBe("products:maxsize:10:page-token:token123");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithFilter()
    {
        var request = new ListProductsRequest { MaxPageSize = 10, Filter = "Category == \"Toys\"" };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5};
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.CacheKey.ShouldBe("products:maxsize:10:filter:category == \"toys\"");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithAllParameters()
    {
        var request = new ListProductsRequest
        {
            MaxPageSize = 10, PageToken = "token123", Filter = "Category == \"Toys\""
        };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5};
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.CacheKey.ShouldBe("products:maxsize:10:page-token:token123:filter:category == \"toys\"");
    }
}
