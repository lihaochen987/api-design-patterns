// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class PersistListProductsToCacheCommandHandlerTests : PersistListProductsToCacheHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldPersistProductsToCache()
    {
        var products = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(5).ToList(),
            NextPageToken = "next-token"
        };
        const string cacheKey = "products:maxsize:10";
        var command = new PersistListProductsToCacheCommand
        {
            Products = products,
            CacheKey = cacheKey,
            Expiry = TimeSpan.FromMinutes(10)
        };
        ICommandHandler<PersistListProductsToCacheCommand> sut = PersistListProductsToCacheHandler();

        await sut.Handle(command);

        var cachedResult = await Cache.GetAsync<CachedItem<ListProductsResponse>>(cacheKey);
        cachedResult.ShouldNotBeNull();
        cachedResult.Item.ShouldNotBeNull();
        cachedResult.Item.Results.Count().ShouldBe(5);
        cachedResult.Item.Results.ToList().ShouldBeEquivalentTo(products.Results.ToList());
        cachedResult.Item.NextPageToken.ShouldBe("next-token");
    }

    [Fact]
    public async Task Handle_ShouldHandleEmptyProductsList()
    {
        var products = new ListProductsResponse
        {
            Results = new List<GetProductResponse>(),
            NextPageToken = null
        };
        const string cacheKey = "products:maxsize:5:filter:category == \"toys\"";
        var command = new PersistListProductsToCacheCommand
        {
            Products = products,
            CacheKey = cacheKey,
            Expiry = TimeSpan.FromMinutes(5)
        };
        ICommandHandler<PersistListProductsToCacheCommand> sut = PersistListProductsToCacheHandler();

        await sut.Handle(command);

        var cachedResult = await Cache.GetAsync<CachedItem<ListProductsResponse>>(cacheKey);
        cachedResult.ShouldNotBeNull();
        cachedResult.Item.ShouldNotBeNull();
        cachedResult.Item.Results.ShouldBeEmpty();
        cachedResult.Item.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCacheFails()
    {
        var products = new ListProductsResponse
        {
            Results = Fixture.CreateMany<GetProductResponse>(5).ToList(),
            NextPageToken = "next-token"
        };
        const string cacheKey = "failing-key";
        var command = new PersistListProductsToCacheCommand
        {
            Products = products,
            CacheKey = cacheKey,
            Expiry = TimeSpan.FromMinutes(10)
        };
        SetupCacheToThrowException(cacheKey);
        ICommandHandler<PersistListProductsToCacheCommand> sut = GetExceptionThrowingHandler();

        await Should.ThrowAsync<Exception>(() => sut.Handle(command));
    }
}
