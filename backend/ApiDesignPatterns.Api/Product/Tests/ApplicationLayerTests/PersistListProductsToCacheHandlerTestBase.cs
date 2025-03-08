// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class PersistListProductsToCacheHandlerTestBase
{
    protected readonly Fixture Fixture = new();
    protected readonly RedisCacheFake Cache = new();
    protected readonly ExceptionThrowingCache ThrowingCache = new();

    protected ICommandHandler<PersistListProductsToCacheCommand> PersistListProductsToCacheHandler()
    {
        return new PersistListProductsToCacheCommandHandler(Cache);
    }

    protected ICommandHandler<PersistListProductsToCacheCommand> GetExceptionThrowingHandler()
    {
        return new PersistListProductsToCacheCommandHandler(ThrowingCache);
    }

    protected void SetupCacheToThrowException(string cacheKey)
    {
        ThrowingCache.SetKeyToThrowOn(cacheKey);
    }
}
