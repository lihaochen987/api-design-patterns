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
            new
            {
                inventory.UserId,
                inventory.ProductId,
                Quantity = inventory.Quantity.Value,
                inventory.RestockDate
            });
    }

    public async Task<DomainModels.Inventory?> GetInventoryByIdAsync(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.Inventory>(InventoryQueries.GetInventoryById,
            new { Id = id });
    }

    public async Task<DomainModels.Inventory?> GetInventoryByProductAndUserAsync(long productId, long userId)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.Inventory>(
            InventoryQueries.GetInventoryByProductAndUser,
            new { ProductId = productId, UserId = userId });
    }

    public async Task UpdateInventoryAsync(DomainModels.Inventory inventory)
    {
        await dbConnection.ExecuteAsync(InventoryQueries.UpdateInventory,
            new { inventory.Id, Quantity = inventory.Quantity.Value, inventory.RestockDate });
    }

    public async Task DeleteInventoryAsync(long id)
    {
        await dbConnection.ExecuteAsync(InventoryQueries.DeleteInventory,
            new { Id = id });
    }
}
