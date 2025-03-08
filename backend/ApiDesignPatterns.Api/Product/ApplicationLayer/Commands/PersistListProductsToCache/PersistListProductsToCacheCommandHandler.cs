// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;

public class PersistListProductsToCacheCommandHandler(
    ICache<CachedItem<ListProductsResponse>> cache)
    : ICommandHandler<PersistListProductsToCacheCommand>
{
    public async Task Handle(PersistListProductsToCacheCommand command)
    {
        var cachedItem = new CachedItem<ListProductsResponse>(command.Products);

        await cache.SetAsync(
            command.CacheKey,
            cachedItem,
            command.Expiry);
    }
}
