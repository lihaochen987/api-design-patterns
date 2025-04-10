// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.Controllers.Product;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class UpdateListProductStalenessHandlerTests : UpdateListProductStalenessHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldIncrementTotalChecksCounter_Always()
    {
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = new ListProductsResponse { Results = new List<GetProductResponse>() },
            FreshResult = new ListProductsResponse { Results = new List<GetProductResponse>() },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandler();

        await sut.Handle(command);

        HashEntry[] hashEntries = await Cache.HashGetAllAsync(StatsKey);
        hashEntries.Length.Should().Be(1);
        hashEntries[0].Name.Should().Be("total_checks");
    }

    [Fact]
    public async Task Handle_ShouldNotIncrementStaleHitsCounter_WhenResultsAreEqual()
    {
        List<GetProductResponse> products = Fixture.CreateMany<GetProductResponse>(3).ToList();
        var response = new ListProductsResponse { Results = products };
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = response,
            FreshResult = new ListProductsResponse { Results = products },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandler();

        await sut.Handle(command);

        HashEntry[] hashEntries = await Cache.HashGetAllAsync(StatsKey);
        hashEntries.Length.Should().Be(1);
        hashEntries[0].Name.Should().Be("total_checks");
    }

    [Fact]
    public async Task Handle_ShouldIncrementStaleHitsCounter_WhenResultsAreDifferent()
    {
        List<GetProductResponse> cachedProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        List<GetProductResponse> freshProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = new ListProductsResponse { Results = cachedProducts },
            FreshResult = new ListProductsResponse { Results = freshProducts },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandler();

        await sut.Handle(command);

        HashEntry[] hashEntries = await Cache.HashGetAllAsync(StatsKey);
        hashEntries.Length.Should().Be(2);
        hashEntries.Count(x => x.Name != "stale_hits").Should().Be(1);
        hashEntries.Count(x => x.Name != "total_checks").Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldLogWarning_WhenStaleRateExceedsMaxAcceptable()
    {
        List<GetProductResponse> cachedProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        List<GetProductResponse> freshProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        await IncrementStatistics(99, 20);
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = new ListProductsResponse { Results = cachedProducts },
            FreshResult = new ListProductsResponse { Results = freshProducts },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandler();

        await sut.Handle(command);

        Mock
            .Get(Logger)
            .Verify(x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("High stale rate detected")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogWarning_WhenStaleRateBelowMinAcceptable()
    {
        var cachedProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        var freshProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        await IncrementStatistics(999, 1);
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = new ListProductsResponse { Results = cachedProducts },
            FreshResult = new ListProductsResponse { Results = freshProducts },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandler();

        await sut.Handle(command);

        Mock
            .Get(Logger)
            .Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Low stale rate detected")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogInformation_WhenStaleRateWithinAcceptableRange()
    {
        var cachedProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        var freshProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        await IncrementStatistics(199, 10);
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = new ListProductsResponse { Results = cachedProducts },
            FreshResult = new ListProductsResponse { Results = freshProducts },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandler();

        await sut.Handle(command);

        Mock
            .Get(Logger)
            .Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Stale data detected")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotLogStatistics_WhenTotalChecksLessThan100()
    {
        List<GetProductResponse> cachedProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        List<GetProductResponse> freshProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        await IncrementStatistics(50, 10);
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = new ListProductsResponse { Results = cachedProducts },
            FreshResult = new ListProductsResponse { Results = freshProducts },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandler();

        await sut.Handle(command);

        Mock
            .Get(Logger)
            .Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("stale rate")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldHandleExceptions_WhenCacheThrowsException()
    {
        List<GetProductResponse> cachedProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        List<GetProductResponse> freshProducts = Fixture.CreateMany<GetProductResponse>(3).ToList();
        ThrowingCache.SetKeyToThrowOn(StatsKey);
        var command = new UpdateListProductStalenessCommand
        {
            CachedResult = new ListProductsResponse { Results = cachedProducts },
            FreshResult = new ListProductsResponse { Results = freshProducts },
            StalenessOptions = new CacheStalenessOptions(TimeSpan.FromMinutes(10), 1, 0.01, 0.1)
        };
        ICommandHandler<UpdateListProductStalenessCommand> sut = GetUpdateListProductStalenessHandlerWithException();

        Func<Task> act = async () => await sut.Handle(command);
        await act.Should().ThrowAsync<Exception>();
    }

    private async Task IncrementStatistics(int totalChecks, int staleHits)
    {
        for (int i = 0; i < totalChecks; i++)
        {
            await Cache.HashIncrementAsync(StatsKey, "total_checks");
        }

        for (int i = 0; i < staleHits; i++)
        {
            await Cache.HashIncrementAsync(StatsKey, "stale_hits");
        }
    }
}
