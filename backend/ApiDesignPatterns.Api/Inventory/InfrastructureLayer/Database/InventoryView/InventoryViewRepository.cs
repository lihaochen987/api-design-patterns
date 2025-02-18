// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Inventory.InfrastructureLayer.Database.InventoryView;

public class InventoryViewRepository(IDbConnection dbConnection): IInventoryViewRepository
{
    public async Task<DomainModels.InventoryView?> GetInventoryView(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.InventoryView>(InventoryViewQueries.GetInventoryView,
            new { Id = id });
    }
}
