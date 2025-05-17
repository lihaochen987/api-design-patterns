// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.DomainModels.ValueObjects;

namespace backend.Inventory.DomainModels;

public record Inventory
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public long ProductId { get; init; }
    public required Quantity Quantity { get; init; }
    public DateTimeOffset? RestockDate { get; init; }
}
