// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.InfrastructureLayer.Database.InventoryView;

public interface IInventoryViewRepository
{
    Task<DomainModels.InventoryView?> GetInventoryView(long id);
}
