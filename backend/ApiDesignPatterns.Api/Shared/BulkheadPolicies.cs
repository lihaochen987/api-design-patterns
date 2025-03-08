// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Bulkhead;

namespace backend.Shared;

public static class BulkheadPolicies
{
    // 100 Max connection pool for PostGres -> Best to move this to a configuration file.
    public static readonly AsyncBulkheadPolicy ProductRead = Policy
        .BulkheadAsync(maxParallelization: 25, maxQueuingActions: 150);

    public static readonly AsyncBulkheadPolicy ProductWrite = Policy
        .BulkheadAsync(maxParallelization: 10, maxQueuingActions: 50);

    public static readonly AsyncBulkheadPolicy InventoryRead = Policy
        .BulkheadAsync(maxParallelization: 15, maxQueuingActions: 75);

    public static readonly AsyncBulkheadPolicy InventoryWrite = Policy
        .BulkheadAsync(maxParallelization: 10, maxQueuingActions: 50);

    public static readonly AsyncBulkheadPolicy ReviewRead = Policy
        .BulkheadAsync(maxParallelization: 10, maxQueuingActions: 50);

    public static readonly AsyncBulkheadPolicy ReviewWrite = Policy
        .BulkheadAsync(maxParallelization: 5, maxQueuingActions: 25);

    public static readonly AsyncBulkheadPolicy SupplierRead = Policy
        .BulkheadAsync(maxParallelization: 15, maxQueuingActions: 75);

    public static readonly AsyncBulkheadPolicy SupplierWrite = Policy
        .BulkheadAsync(maxParallelization: 5, maxQueuingActions: 25);

    // 10,000 MaxClients for Redis -> Best to move this to a configuration file
    public static readonly AsyncBulkheadPolicy RedisRead = Policy
        .BulkheadAsync(maxParallelization: 6300, maxQueuingActions: 31500);

    public static readonly AsyncBulkheadPolicy RedisWrite = Policy
        .BulkheadAsync(maxParallelization: 3150, maxQueuingActions: 15750);

}
