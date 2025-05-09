// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponses;
using backend.Product.Tests.TestHelpers.Fakes.BatchCreateProductCacheFake;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class CacheCreateProductResponsesHandlerTestBase
{
    protected readonly Fixture Fixture = new();
    protected readonly BatchCreateProductCacheFake Cache = new();
    private readonly BatchCreateProductsExceptionThrowingCacheFake _throwingCache = new();

    protected ICommandHandler<CacheCreateProductResponsesCommand> CacheCreateProductResponsesHandler()
    {
        return new CacheCreateProductResponsesHandler(Cache);
    }

    protected ICommandHandler<CacheCreateProductResponsesCommand> GetExceptionThrowingHandler()
    {
        return new CacheCreateProductResponsesHandler(_throwingCache);
    }

    protected void SetupCacheToThrowException(string cacheKey)
    {
        _throwingCache.SetKeyToThrowOn(cacheKey);
    }
}
