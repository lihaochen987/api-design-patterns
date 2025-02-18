// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.InventoryControllers;

public record GetInventoryRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}
