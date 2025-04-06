// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Product.Tests.TestHelpers.Fakes.ListProductsCacheFake;
using backend.Product.Tests.TestHelpers.Fakes.ListProductsExceptionThrowingCacheFake;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class PersistListProductsToCacheHandlerTestBase
{
    protected readonly Fixture Fixture = new();
    protected readonly ListProductsCacheFake Cache = new();
    private readonly ListProductsExceptionThrowingCacheFake _throwingCache = new();

    protected ICommandHandler<PersistListProductsToCacheCommand> PersistListProductsToCacheHandler()
    {
        return new PersistListProductsToCacheCommandHandler(Cache);
    }

    protected ICommandHandler<PersistListProductsToCacheCommand> GetExceptionThrowingHandler()
    {
        return new PersistListProductsToCacheCommandHandler(_throwingCache);
    }

    protected void SetupCacheToThrowException(string cacheKey)
    {
        _throwingCache.SetKeyToThrowOn(cacheKey);
    }
}
