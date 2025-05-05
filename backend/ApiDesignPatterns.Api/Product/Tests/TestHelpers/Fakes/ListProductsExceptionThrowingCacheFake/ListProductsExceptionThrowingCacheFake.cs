// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.InfrastructureLayer.Cache;
using backend.Shared.Caching;
using StackExchange.Redis;

namespace backend.Product.Tests.TestHelpers.Fakes.ListProductsExceptionThrowingCacheFake;

public class ListProductsExceptionThrowingCacheFake : IListProductsCache
{
    public readonly HashSet<string> KeysToThrowOn = [];

    public void SetKeyToThrowOn(string key)
    {
        KeysToThrowOn.Add(key);
    }

    public Task SetAsync(string key, CachedItem<ListProductsResponse> value, TimeSpan expiry)
    {
        if (KeysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.CompletedTask;
    }

    public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
    {
        if (KeysToThrowOn.Contains(key!))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.FromResult(Array.Empty<HashEntry>());
    }

    public Task<BatchResult> ExecuteBatchAsync(Action<IBatchOperations> batchAction)
    {
        var operations = new ExceptionThrowingBatchOperations(this);
        var result = new BatchResult();

        try
        {
            batchAction(operations);
            result.HashIncrements = operations.HashIncrements;
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            return Task.FromException<BatchResult>(ex);
        }
    }

    public Task<CachedItem<ListProductsResponse>?> GetAsync(string key)
    {
        if (KeysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        return Task.FromResult<CachedItem<ListProductsResponse>?>(null);
    }
}
