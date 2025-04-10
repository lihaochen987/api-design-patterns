// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.Controllers;

public class UpdateInventoryRequest
{
    public decimal? Quantity { get; init; }
    public DateTimeOffset? RestockDate { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
