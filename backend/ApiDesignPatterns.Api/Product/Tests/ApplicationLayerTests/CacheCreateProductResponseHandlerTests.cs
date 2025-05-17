// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponse;
using backend.Product.Controllers.Product;
using backend.Shared;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using backend.Shared.Utility;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class CacheCreateProductResponseHandlerTests : CacheCreateProductResponseHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldCacheProductResponse()
    {
        var request = Fixture.Create<CreateProductRequest>();
        var response = Fixture.Create<CreateProductResponse>();
        string requestId = Guid.NewGuid().ToString();
        var command = new CacheCreateProductResponseCommand
        {
            RequestId = requestId, CreateProductRequest = request, CreateProductResponse = response
        };
        ICommandHandler<CacheCreateProductResponseCommand> sut = CacheCreateProductResponseHandler();

        await sut.Handle(command);

        string expectedHash = ObjectHasher.ComputeHash(request);
        CachedItem<CreateProductResponse>? cachedResult = await Cache.GetAsync(requestId);
        cachedResult.Should().NotBeNull();
        cachedResult.Hash.Should().Be(expectedHash);
        cachedResult.Item.Should().BeEquivalentTo(response);
        cachedResult.Timestamp.Should()
            .BeCloseTo(DateTime.UtcNow + TimeSpan.FromSeconds(5), precision: TimeSpan.FromSeconds(1));
    }


    [Fact]
    public async Task Handle_ShouldThrowException_WhenCacheFails()
    {
        var request = Fixture.Create<CreateProductRequest>();
        var response = Fixture.Create<CreateProductResponse>();
        const string requestId = "failing-key";
        var command = new CacheCreateProductResponseCommand
        {
            RequestId = requestId, CreateProductRequest = request, CreateProductResponse = response
        };
        SetupCacheToThrowException(requestId);
        ICommandHandler<CacheCreateProductResponseCommand> sut = GetExceptionThrowingHandler();

        Func<Task> act = async () => await sut.Handle(command);

        await act.Should().ThrowAsync<Exception>();
    }
}
