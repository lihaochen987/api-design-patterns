// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;
using backend.Product.Tests.TestHelpers.Fakes.ListProductsCacheFake;
using backend.Product.Tests.TestHelpers.Fakes.ListProductsExceptionThrowingCacheFake;
using backend.Shared.CommandHandler;
using Moq;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class UpdateListProductStalenessHandlerTestBase
{
    protected readonly ListProductsCacheFake Cache = new();
    protected readonly ListProductsExceptionThrowingCacheFake ThrowingCache = new();

    protected readonly ILogger<UpdateListProductStalenessHandler> Logger =
        Mock.Of<ILogger<UpdateListProductStalenessHandler>>();
    protected const string StatsKey = $"cache:stats:{nameof(UpdateListProductStalenessHandler)}";
    protected readonly Fixture Fixture = new();

    protected ICommandHandler<UpdateListProductStalenessCommand> GetUpdateListProductStalenessHandler()
    {
        return new UpdateListProductStalenessHandler(Cache, Logger);
    }

    protected ICommandHandler<UpdateListProductStalenessCommand> GetUpdateListProductStalenessHandlerWithException()
    {
        return new UpdateListProductStalenessHandler(ThrowingCache, Logger);
    }
}
