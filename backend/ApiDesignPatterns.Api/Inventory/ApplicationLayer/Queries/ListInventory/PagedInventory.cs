// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.DomainModels;

namespace backend.Inventory.ApplicationLayer.Queries.ListInventory;

public record PagedInventory(List<InventoryView> Inventory, string? NextPageToken);
