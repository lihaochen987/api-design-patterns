// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.InfrastructureLayer.Database.Inventory;

public interface IInventoryRepository
{
    Task CreateInventoryAsync(DomainModels.Inventory inventory);
    Task<DomainModels.Inventory?> GetInventoryByIdAsync(long id);
    Task UpdateInventoryAsync(DomainModels.Inventory inventory);
    Task<DomainModels.Inventory?> GetInventoryByProductAndUserAsync(long productId, long userId);
    Task DeleteInventoryAsync(long id);
}
