// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Inventory.DomainModels;

public record InventoryView : Identifier
{
    public long SupplierId { get; set; }
    public long ProductId { get; init; }
    public decimal Quantity { get; init; }
    public DateTimeOffset? RestockDate { get; init; }
}
