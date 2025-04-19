// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetInventoryById;

public class GetInventoryByIdByIdHandler(IInventoryRepository repository) : IQueryHandler<GetInventoryByIdQuery, DomainModels.Inventory?>
{
    public async Task<DomainModels.Inventory?> Handle(GetInventoryByIdQuery byIdQuery)
    {
        DomainModels.Inventory? inventory = await repository.GetInventoryByIdAsync(byIdQuery.Id);

        return inventory ?? null;
    }
}
