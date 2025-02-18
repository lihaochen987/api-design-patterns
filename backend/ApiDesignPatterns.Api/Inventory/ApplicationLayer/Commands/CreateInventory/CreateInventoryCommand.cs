// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.ApplicationLayer.Commands.CreateInventory;

public record CreateInventoryCommand
{
    public required DomainModels.Inventory Inventory { get; init; }
}
