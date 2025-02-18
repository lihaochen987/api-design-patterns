// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.DomainModels;

public record Inventory
{
    public long Id { get; init; }
    public long SupplierId { get; init; }
    public long ProductId { get; init; }
    public decimal Quantity { get; init; }
    public DateTimeOffset? RestockDate { get; init; }
}
