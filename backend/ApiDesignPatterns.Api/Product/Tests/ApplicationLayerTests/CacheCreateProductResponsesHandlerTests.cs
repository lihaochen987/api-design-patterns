// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponses;
using backend.Product.Controllers.Product;
using backend.Shared;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using backend.Shared.Utility;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class CacheCreateProductResponsesHandlerTests : CacheCreateProductResponsesHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldCacheMultipleProductResponses()
    {
        var requests = Fixture.CreateMany<CreateProductRequest>(3).ToList();
        var responses = Fixture.CreateMany<CreateProductResponse>(3).ToList();
        string requestId = Guid.NewGuid().ToString();
        var command = new CacheCreateProductResponsesCommand
        {
            RequestId = requestId, CreateProductRequests = requests, CreateProductResponses = responses
        };
        ICommandHandler<CacheCreateProductResponsesCommand> sut = CacheCreateProductResponsesHandler();

        await sut.Handle(command);

        string expectedHash = ObjectHasher.ComputeHash(requests);
        CachedItem<IEnumerable<CreateProductResponse>>? cachedResult = await Cache.GetAsync(requestId);
        cachedResult.Should().NotBeNull();
        cachedResult.Hash.Should().Be(expectedHash);
        cachedResult.Item.Should().BeEquivalentTo(responses);
        cachedResult.Timestamp.Should()
            .BeCloseTo(DateTime.UtcNow + TimeSpan.FromSeconds(5), precision: TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Handle_ShouldCacheEmptyResponses_WhenNoProductsProvided()
    {
        var requests = new List<CreateProductRequest>();
        var responses = new List<CreateProductResponse>();
        string requestId = Guid.NewGuid().ToString();
        var command = new CacheCreateProductResponsesCommand
        {
            RequestId = requestId, CreateProductRequests = requests, CreateProductResponses = responses
        };
        ICommandHandler<CacheCreateProductResponsesCommand> sut = CacheCreateProductResponsesHandler();

        await sut.Handle(command);

        string expectedHash = ObjectHasher.ComputeHash(requests);
        CachedItem<IEnumerable<CreateProductResponse>>? cachedResult = await Cache.GetAsync(requestId);
        cachedResult.Should().NotBeNull();
        cachedResult.Hash.Should().Be(expectedHash);
        cachedResult.Item.Should().BeEquivalentTo(responses);
        cachedResult.Timestamp.Should()
            .BeCloseTo(DateTime.UtcNow + TimeSpan.FromSeconds(5), precision: TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCacheFails()
    {
        var requests = Fixture.CreateMany<CreateProductRequest>(3).ToList();
        var responses = Fixture.CreateMany<CreateProductResponse>(3).ToList();
        string requestId = Fixture.Create<string>();
        var command = new CacheCreateProductResponsesCommand
        {
            RequestId = requestId, CreateProductRequests = requests, CreateProductResponses = responses
        };
        SetupCacheToThrowException(requestId);
        ICommandHandler<CacheCreateProductResponsesCommand> sut = GetExceptionThrowingHandler();

        Func<Task> act = async () => await sut.Handle(command);

        await act.Should().ThrowAsync<Exception>();
    }
}
