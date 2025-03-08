// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.ProductControllers;
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
        var query = new GetListProductsFromCacheQuery { Request = request };
        var expectedResponse = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(10), NextPageToken = "11"
        };
        await SetupCacheWithData("products:maxsize:10", expectedResponse);
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ProductsResponse.ShouldNotBeNull();
        result.ProductsResponse.Results.Count().ShouldBe(10);
        result.ProductsResponse.Results.ShouldBeEquivalentTo(expectedResponse.Results.ToList());
        result.cacheKey.ShouldBe("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldReturnNullProductsResponse_WhenCacheDoesNotHaveData()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request };
        SetupCacheWithNoData("products:maxsize:10");
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ProductsResponse.ShouldBeNull();
        result.cacheKey.ShouldBe("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldReturnNullProductsResponse_WhenCacheThrowsException()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request };
        SetupCacheToThrowException("products:maxsize:10");
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetExceptionThrowingHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ProductsResponse.ShouldBeNull();
        result.cacheKey.ShouldBe("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithPageToken()
    {
        var request = new ListProductsRequest { MaxPageSize = 10, PageToken = "token123" };
        var query = new GetListProductsFromCacheQuery { Request = request };
        SetupCacheWithNoData("products:maxsize:10:page-token:token123");
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.cacheKey.ShouldBe("products:maxsize:10:page-token:token123");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithFilter()
    {
        var request = new ListProductsRequest { MaxPageSize = 10, Filter = "Category == \"Toys\"" };
        var query = new GetListProductsFromCacheQuery { Request = request };
        SetupCacheWithNoData("products:maxsize:10:filter:category == \"toys\"");
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.cacheKey.ShouldBe("products:maxsize:10:filter:category == \"toys\"");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithAllParameters()
    {
        var request = new ListProductsRequest
        {
            MaxPageSize = 10, PageToken = "token123", Filter = "Category == \"Toys\""
        };
        var query = new GetListProductsFromCacheQuery { Request = request };
        SetupCacheWithNoData("products:maxsize:10:page-token:token123:filter:category == \"toys\"");
        IQueryHandler<GetListProductsFromCacheQuery, CacheQueryResult> sut = GetListProductsFromCacheHandler();

        CacheQueryResult? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.cacheKey.ShouldBe("products:maxsize:10:page-token:token123:filter:category == \"toys\"");
    }
}
