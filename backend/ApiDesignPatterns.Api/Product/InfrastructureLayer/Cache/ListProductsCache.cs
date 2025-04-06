// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using backend.Product.ProductControllers;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.InfrastructureLayer.Cache;

public class ListProductsCache(IDatabase redisDatabase) : IListProductsCache
{
    public async Task<CachedItem<ListProductsResponse>?> GetAsync(string key)
    {
        RedisValue cachedValue = await redisDatabase.StringGetAsync(key);

        return cachedValue.IsNull ? null : JsonSerializer.Deserialize<CachedItem<ListProductsResponse>>(cachedValue!);
    }

    public async Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        string serializedValue = JsonSerializer.Serialize(value);
        await redisDatabase.StringSetAsync(key, serializedValue, expiry);
    }

    public IBatch CreateBatch()
    {
        return redisDatabase.CreateBatch();
    }

    public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
    {
        return redisDatabase.HashGetAllAsync(key, flags);
    }

    public Task<long> HashIncrementAsync(string key, string field)
    {
        return redisDatabase.HashIncrementAsync(key, field);
    }

    public Task<BatchResult> ExecuteBatchAsync(Action<IBatchOperations> batchAction)
    {
        var batch = redisDatabase.CreateBatch();
        var operations = new RedisBatchOperations(batch);
        var result = new BatchResult();

        batchAction(operations);

        result.HashIncrements = operations.HashIncrements;

        batch.Execute();

        return Task.FromResult(result);
    }

    private class RedisBatchOperations(IBatch batch) : IBatchOperations
    {
        public Dictionary<string, Task<long>> HashIncrements { get; } = new();

        public void HashIncrementAsync(string key, string field)
        {
            var task = batch.HashIncrementAsync(key, field);
            string operationKey = $"{key}:{field}";
            HashIncrements[operationKey] = task;
        }
    }
}
