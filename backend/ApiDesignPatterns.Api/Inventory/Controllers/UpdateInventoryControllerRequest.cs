// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.Controllers;

public class UpdateInventoryControllerRequest
{
    public string? SupplierId { get; init; }
    public string? ProductId { get; init; }
    public decimal? Quantity { get; init; }
    public string? RestockDate { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
