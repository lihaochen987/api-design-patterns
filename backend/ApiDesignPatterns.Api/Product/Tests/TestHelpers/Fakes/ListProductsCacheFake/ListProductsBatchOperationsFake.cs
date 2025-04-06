// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.Caching;

namespace backend.Product.Tests.TestHelpers.Fakes.ListProductsCacheFake;

public class ListProductsBatchOperationsFake(ListProductsCacheFake cache) : IBatchOperations
{
    public Dictionary<string, Task<long>> HashIncrements { get; } = new();

    public void HashIncrementAsync(string key, string field)
    {
        var task = cache.HashIncrementAsync(key, field);
        string operationKey = $"{key}:{field}";
        HashIncrements[operationKey] = task;
    }
}
