// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.Controllers;

namespace backend.Inventory.ApplicationLayer.Commands.UpdateInventory;

public record UpdateInventoryCommand
{
    public required UpdateInventoryRequest Request { get; init; }
    public required DomainModels.Inventory Inventory { get; init; }
}
