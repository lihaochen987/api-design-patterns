// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.CacheCreateProductResponse;
using backend.Product.Tests.TestHelpers.Fakes.CreateProductCacheFake;
using backend.Shared.CommandHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class CacheCreateProductResponseHandlerTestBase
{
    protected readonly Fixture Fixture = new();
    protected readonly CreateProductCacheFake Cache = new();
    private readonly CreateProductExceptionThrowingCacheFake _throwingCache = new();

    protected ICommandHandler<CacheCreateProductResponseCommand> CacheCreateProductResponseHandler()
    {
        return new CacheCreateProductResponseHandler(Cache);
    }

    protected ICommandHandler<CacheCreateProductResponseCommand> GetExceptionThrowingHandler()
    {
        return new CacheCreateProductResponseHandler(_throwingCache);
    }

    protected void SetupCacheToThrowException(string cacheKey)
    {
        _throwingCache.SetKeyToThrowOn(cacheKey);
    }
}
