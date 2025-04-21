// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;
using backend.Product.Controllers.Product;
using backend.Shared;
using backend.Shared.Caching;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class GetListProductsFromCacheHandlerTests : GetListProductsFromCacheHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheHasData()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5 };
        var expectedResults = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(10), NextPageToken = "11"
        };
        var expectedResponse = new CachedItem<ListProductsResponse>
        {
            Item = expectedResults, Timestamp = DateTime.UtcNow + TimeSpan.FromMinutes(10)
        };
        await Cache.SetAsync("products:maxsize:10", expectedResponse, TimeSpan.FromMinutes(10));
        var sut = GetListProductsFromCacheHandler();

        CacheQueryResult result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.ProductsResponse.Should().NotBeNull();
        result.ProductsResponse.Results.Count().Should().Be(10);
        result.ProductsResponse.Results.Should().BeEquivalentTo(expectedResponse.Item.Results);
        result.CacheKey.Should().Be("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldReturnNullProductsResponse_WhenCacheDoesNotHaveData()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5 };
        var sut = GetListProductsFromCacheHandler();

        CacheQueryResult result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.ProductsResponse.Should().BeNull();
        result.CacheKey.Should().Be("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldReturnNullProductsResponse_WhenCacheThrowsException()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5 };
        var sut = GetExceptionThrowingHandler();
        ThrowingCache.SetKeyToThrowOn("products:maxsize:10");

        CacheQueryResult result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.ProductsResponse.Should().BeNull();
        result.CacheKey.Should().Be("products:maxsize:10");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithPageToken()
    {
        var request = new ListProductsRequest { MaxPageSize = 10, PageToken = "token123" };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5 };
        var sut = GetListProductsFromCacheHandler();

        CacheQueryResult result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.CacheKey.Should().Be("products:maxsize:10:page-token:token123");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithFilter()
    {
        var request = new ListProductsRequest { MaxPageSize = 10, Filter = "Category == \"Toys\"" };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5 };
        var sut = GetListProductsFromCacheHandler();

        CacheQueryResult result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.CacheKey.Should().Be("products:maxsize:10:filter:category == \"toys\"");
    }

    [Fact]
    public async Task Handle_ShouldGenerateCorrectCacheKey_WithAllParameters()
    {
        var request = new ListProductsRequest
        {
            MaxPageSize = 10, PageToken = "token123", Filter = "Category == \"Toys\""
        };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 5 };
        var sut = GetListProductsFromCacheHandler();

        CacheQueryResult result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.CacheKey.Should().Be("products:maxsize:10:page-token:token123:filter:category == \"toys\"");
    }

    [Fact]
    public async Task Handle_ShouldSetSelectedForStalenessCheckToTrue_WhenRandomCheckPasses()
    {
        var request = new ListProductsRequest { MaxPageSize = 10 };
        var query = new GetListProductsFromCacheQuery { Request = request, CheckRate = 0 };
        var expectedResults = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(10),
            NextPageToken = "11"
        };
        var expectedResponse = new CachedItem<ListProductsResponse>
        {
            Item = expectedResults,
            Timestamp = DateTime.UtcNow + TimeSpan.FromMinutes(10)
        };
        await Cache.SetAsync("products:maxsize:10", expectedResponse, TimeSpan.FromMinutes(10));
        RandomUtility.CheckProbability(0);
        var sut = GetListProductsFromCacheHandler();

        CacheQueryResult result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.ProductsResponse.Should().NotBeNull();
        result.CacheKey.Should().Be("products:maxsize:10");
        result.SelectedForStalenessCheck.Should().BeFalse();
    }
}
