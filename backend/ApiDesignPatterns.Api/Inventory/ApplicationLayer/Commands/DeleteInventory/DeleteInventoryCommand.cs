// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.ApplicationLayer.Commands.DeleteInventory;

public record DeleteInventoryCommand
{
    public long Id { get; init; }
}
