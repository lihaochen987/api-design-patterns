// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using StackExchange.Redis;

namespace backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;

public class PersistListProductsToCacheCommandHandler(
    IDatabase redisCache)
    : ICommandHandler<PersistListProductsToCacheCommand>
{
    public async Task Handle(PersistListProductsToCacheCommand command)
    {
        var cachedItem = new CachedItem<ListProductsResponse>(command.Products);

        string serializedData = JsonSerializer.Serialize(cachedItem);

        if (command.Expiry.HasValue)
        {
            await redisCache.StringSetAsync(
                command.CacheKey,
                serializedData,
                command.Expiry);
        }
        else
        {
            await redisCache.StringSetAsync(
                command.CacheKey,
                serializedData);
        }
    }
}
