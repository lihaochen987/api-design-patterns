// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using backend.Shared.Caching;
using backend.Shared.Exceptions;
using StackExchange.Redis;

namespace backend.Shared.CommandHandler;

public class RedisCacheCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    IDatabase redisCache,
    TimeSpan cacheExpiration,
    ILogger<RedisCacheCommandHandlerDecorator<TCommand>> logger)
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

        var transaction = redisCache.CreateTransaction();

        var existingValue = await redisCache.StringGetAsync(cacheKey);
        if (existingValue.HasValue)
        {
            var cachedResponse = JsonSerializer.Deserialize<CachedResponse<object>>(existingValue.ToString());
            if (cachedResponse?.RequestHash != requestHash)
            {
                logger.LogWarning("Request ID collision detected for command {CommandType}", typeof(TCommand).Name);
                throw new RequestIdCollisionException("Request ID collision detected");
            }

            await RefreshCacheEntry(cacheKey, cachedResponse, transaction,cacheExpiration);
        }

        await commandHandler.Handle(command);

        await CacheCommandResponse(cacheKey, requestHash, cacheExpiration);
    }

    private async Task RefreshCacheEntry(
        string cacheKey,
        CachedResponse<object> cachedResponse,
        ITransaction transaction,
        TimeSpan expiration)
    {
        var refreshedResponse = cachedResponse with { LastAccessed = DateTime.UtcNow };

        _ = transaction.StringSetAsync(
            cacheKey,
            JsonSerializer.Serialize(refreshedResponse),
            expiration
        );

        if (await transaction.ExecuteAsync())
        {
            logger.LogInformation("Skipping duplicate command execution for {CommandType}", typeof(TCommand).Name);
        }
    }

    private async Task CacheCommandResponse(string cacheKey, string requestHash, TimeSpan expiration)
    {
        var responseToCache = new CachedResponse<object>
        {
            Response = null, RequestHash = requestHash, LastAccessed = DateTime.UtcNow
        };

        await redisCache.StringSetAsync(
            cacheKey,
            JsonSerializer.Serialize(responseToCache),
            expiration
        );
    }

    private static string CalculateHash(TCommand command)
    {
        string commandJson = JsonSerializer.Serialize(command);
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(commandJson));
        return Convert.ToBase64String(hashBytes);
    }
}
