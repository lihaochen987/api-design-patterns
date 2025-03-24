// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class PersistListProductsToCacheCommandHandlerTests : PersistListProductsToCacheHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldPersistProductsToCache()
    {
        var products = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(5).ToList(), NextPageToken = "next-token"
        };
        const string cacheKey = "products:maxsize:10";
        var command = new PersistListProductsToCacheCommand
        {
            Products = products, CacheKey = cacheKey, Expiry = TimeSpan.FromMinutes(10)
        };
        ICommandHandler<PersistListProductsToCacheCommand> sut = PersistListProductsToCacheHandler();

        await sut.Handle(command);

        CachedItem<ListProductsResponse>? cachedResult = await Cache.GetAsync(cacheKey);
        cachedResult.Should().NotBeNull();
        cachedResult.Item.Should().NotBeNull();
        cachedResult.Item.Results.Count().Should().Be(5);
        cachedResult.Item.Results.ToList().Should().BeEquivalentTo(products.Results.ToList());
        cachedResult.Item.NextPageToken.Should().Be("next-token");
    }

    [Fact]
    public async Task Handle_ShouldHandleEmptyProductsList()
    {
        var products = new ListProductsResponse { Results = new List<GetProductResponse>(), NextPageToken = null };
        const string cacheKey = "products:maxsize:5:filter:category == \"toys\"";
        var command = new PersistListProductsToCacheCommand
        {
            Products = products, CacheKey = cacheKey, Expiry = TimeSpan.FromMinutes(5)
        };
        ICommandHandler<PersistListProductsToCacheCommand> sut = PersistListProductsToCacheHandler();

        await sut.Handle(command);

        CachedItem<ListProductsResponse>? cachedResult = await Cache.GetAsync(cacheKey);
        cachedResult.Should().NotBeNull();
        cachedResult.Item.Should().NotBeNull();
        cachedResult.Item.Results.Should().BeEmpty();
        cachedResult.Item.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCacheFails()
    {
        var products = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(5).ToList(), NextPageToken = "next-token"
        };
        const string cacheKey = "failing-key";
        var command = new PersistListProductsToCacheCommand
        {
            Products = products, CacheKey = cacheKey, Expiry = TimeSpan.FromMinutes(10)
        };
        SetupCacheToThrowException(cacheKey);
        ICommandHandler<PersistListProductsToCacheCommand> sut = GetExceptionThrowingHandler();

        Func<Task> act = async () => await sut.Handle(command);

        await act.Should().ThrowAsync<Exception>();
    }
}
