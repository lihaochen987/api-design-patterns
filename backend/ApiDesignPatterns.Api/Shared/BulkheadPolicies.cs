// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Bulkhead;

namespace backend.Shared;

public static class BulkheadPolicies
{
    // 100 Max connection pool for PostGres -> Best to move this to a configuration file.
    public static readonly AsyncBulkheadPolicy ProductRead = Policy
        .BulkheadAsync(maxParallelization: 20, maxQueuingActions: 120);

    public static readonly AsyncBulkheadPolicy ProductWrite = Policy
        .BulkheadAsync(maxParallelization: 8, maxQueuingActions: 40);

    public static readonly AsyncBulkheadPolicy InventoryRead = Policy
        .BulkheadAsync(maxParallelization: 12, maxQueuingActions: 60);

    public static readonly AsyncBulkheadPolicy InventoryWrite = Policy
        .BulkheadAsync(maxParallelization: 8, maxQueuingActions: 40);

    public static readonly AsyncBulkheadPolicy ReviewRead = Policy
        .BulkheadAsync(maxParallelization: 8, maxQueuingActions: 40);

    public static readonly AsyncBulkheadPolicy ReviewWrite = Policy
        .BulkheadAsync(maxParallelization: 4, maxQueuingActions: 20);

    public static readonly AsyncBulkheadPolicy UserRead = Policy
        .BulkheadAsync(maxParallelization: 12, maxQueuingActions: 60);

    public static readonly AsyncBulkheadPolicy UserWrite = Policy
        .BulkheadAsync(maxParallelization: 4, maxQueuingActions: 20);

    public static readonly AsyncBulkheadPolicy PhoneNumberRead = Policy
        .BulkheadAsync(maxParallelization: 10, maxQueuingActions: 50);

    public static readonly AsyncBulkheadPolicy PhoneNumberWrite = Policy
        .BulkheadAsync(maxParallelization: 4, maxQueuingActions: 20);

    public static readonly AsyncBulkheadPolicy AddressRead = Policy
        .BulkheadAsync(maxParallelization: 10, maxQueuingActions: 50);

    public static readonly AsyncBulkheadPolicy AddressWrite = Policy
        .BulkheadAsync(maxParallelization: 4, maxQueuingActions: 20);

    // 10,000 MaxClients for Redis -> Best to move this to a configuration file
    public static readonly AsyncBulkheadPolicy RedisRead = Policy
        .BulkheadAsync(maxParallelization: 6300, maxQueuingActions: 31500);

    public static readonly AsyncBulkheadPolicy RedisWrite = Policy
        .BulkheadAsync(maxParallelization: 3150, maxQueuingActions: 15750);
}
