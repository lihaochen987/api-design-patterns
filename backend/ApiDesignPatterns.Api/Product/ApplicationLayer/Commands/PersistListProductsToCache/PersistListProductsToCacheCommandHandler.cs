// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Cache;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;

public class PersistListProductsToCacheCommandHandler(
    IListProductsCache cache)
    : ICommandHandler<PersistListProductsToCacheCommand>
{
    public async Task Handle(PersistListProductsToCacheCommand command)
    {
        var cachedItem =
            new CachedItem<ListProductsResponse>
            {
                Item = command.Products, Timestamp = DateTime.UtcNow + command.Expiry
            };

        await cache.SetAsync(
            command.CacheKey,
            cachedItem,
            command.Expiry);
    }
}
