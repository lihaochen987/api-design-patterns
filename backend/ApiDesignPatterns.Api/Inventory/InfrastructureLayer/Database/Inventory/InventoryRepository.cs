// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Inventory.InfrastructureLayer.Database.Inventory;

public class InventoryRepository(IDbConnection dbConnection) : IInventoryRepository
{
    public async Task CreateInventoryAsync(DomainModels.Inventory inventory)
    {
        await dbConnection.ExecuteAsync(InventoryQueries.CreateInventory,
            new { inventory.SupplierId, inventory.ProductId, inventory.Quantity, inventory.RestockDate });
    }

    public async Task<DomainModels.Inventory?> GetInventoryAsync(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.Inventory>(InventoryQueries.GetInventory,
            new { Id = id });
    }

    public async Task UpdateInventoryAsync(DomainModels.Inventory inventory)
    {
        await dbConnection.ExecuteAsync(InventoryQueries.UpdateInventory,
            new { inventory.Id, inventory.Quantity, inventory.RestockDate });
    }
}
