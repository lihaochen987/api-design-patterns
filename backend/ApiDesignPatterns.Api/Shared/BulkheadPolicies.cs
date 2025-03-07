// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Polly;
using Polly.Bulkhead;

namespace backend.Shared;

public static class BulkheadPolicies
{
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

}
