// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.InventoryView;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetInventoryView;

public class GetInventoryViewHandler(IInventoryViewRepository repository)
    : IAsyncQueryHandler<GetInventoryViewQuery, InventoryView?>
{
    public async Task<InventoryView?> Handle(GetInventoryViewQuery query)
    {
        InventoryView? inventory = await repository.GetInventoryView(query.Id);
        return inventory;
    }
}
