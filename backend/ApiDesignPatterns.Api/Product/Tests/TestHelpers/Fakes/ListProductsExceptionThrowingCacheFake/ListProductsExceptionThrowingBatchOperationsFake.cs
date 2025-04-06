// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes.ListProductsExceptionThrowingCacheFake;

public class ExceptionThrowingBatchOperations(ListProductsExceptionThrowingCacheFake cache) : IBatchOperations
{
    public Dictionary<string, Task<long>> HashIncrements { get; } = new();

    public void HashIncrementAsync(string key, string field)
    {
        if (cache.KeysToThrowOn.Contains(key))
        {
            throw new Exception($"Simulated cache error for key: {key}");
        }

        var task = Task.FromResult(1L);
        string operationKey = $"{key}:{field}";
        HashIncrements[operationKey] = task;
    }
}
