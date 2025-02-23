// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using backend.Shared.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace backend.Shared.CommandHandler;

public class CacheCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    IMemoryCache cache,
    TimeSpan cacheExpiration,
    ILogger<CacheCommandHandlerDecorator<TCommand>> logger)
    : ICommandHandler<TCommand> where TCommand : ICachedCommand
{
    public async Task Handle(TCommand command)
    {
        if (string.IsNullOrEmpty(command.RequestId))
        {
            await commandHandler.Handle(command);
            return;
        }

        string requestHash = CalculateHash(command);
        string cacheKey = $"command:{command.RequestId}";

        if (cache.TryGetValue<CachedResponse<object>>(cacheKey, out var cachedResponse))
        {
            if (cachedResponse?.RequestHash != requestHash)
            {
                logger.LogWarning("Request ID collision detected for command {CommandType}", typeof(TCommand).Name);
                throw new RequestIdCollisionException("Request ID collision detected");
            }

            var refreshedResponse = cachedResponse with { LastAccessed = DateTime.UtcNow };
            var options = new MemoryCacheEntryOptions().SetSlidingExpiration(cacheExpiration);
            cache.Set(cacheKey, refreshedResponse, options);

            logger.LogInformation("Skipping duplicate command execution for {CommandType}", typeof(TCommand).Name);
            return;
        }

        await commandHandler.Handle(command);

        var responseToCache = new CachedResponse<object>
        {
            Response = null, RequestHash = requestHash, LastAccessed = DateTime.UtcNow
        };

        var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(cacheExpiration);

        cache.Set(cacheKey, responseToCache, cacheOptions);
    }

    private static string CalculateHash(TCommand command)
    {
        string commandJson = JsonSerializer.Serialize(command);
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(commandJson));
        return Convert.ToBase64String(hashBytes);
    }
}
