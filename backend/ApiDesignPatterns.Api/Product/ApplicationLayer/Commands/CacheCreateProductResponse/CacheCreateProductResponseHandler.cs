// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;
using backend.Shared.CommandHandler;
using StackExchange.Redis;

namespace backend.Product.ApplicationLayer.Commands.CacheCreateProductResponse;

public class CacheCreateProductResponseHandler(ICreateProductCache cache)
    : ICommandHandler<CacheCreateProductResponseCommand>
{
    public async Task Handle(CacheCreateProductResponseCommand command)
    {
        string hash = GenerateHash(command.CreateProductRequest);
        var cachedItem = new CachedItem<CreateProductResponse>
        {
            Hash = hash, Item = command.CreateProductResponse, Timestamp = DateTime.UtcNow + TimeSpan.FromSeconds(5)
        };

        await cache.SetAsync(command.RequestId, cachedItem, TimeSpan.FromSeconds(5));
    }

    private static string GenerateHash<T>(T obj)
    {
        string json = System.Text.Json.JsonSerializer.Serialize(obj);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(json));

        return Convert.ToBase64String(hashBytes);
    }
}
