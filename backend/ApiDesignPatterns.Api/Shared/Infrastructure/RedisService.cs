// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using StackExchange.Redis;

namespace backend.Shared.Infrastructure;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _redis;

    public RedisService(IConfiguration configuration)
    {
        string? redisConnection = configuration["REDIS_CONNECTION"];
        _redis = ConnectionMultiplexer.Connect(redisConnection ?? throw new InvalidOperationException());
    }

    public IDatabase GetDatabase()
    {
        return _redis.GetDatabase();
    }
}
