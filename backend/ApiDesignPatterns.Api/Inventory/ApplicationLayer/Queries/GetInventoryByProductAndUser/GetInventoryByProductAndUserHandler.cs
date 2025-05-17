// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndUser;

public class GetInventoryByProductAndUserHandler(IInventoryRepository repository)
    : IAsyncQueryHandler<GetInventoryByProductAndUserQuery, DomainModels.Inventory?>
{
    public Task<DomainModels.Inventory?> Handle(GetInventoryByProductAndUserQuery query)
    {
        return repository.GetInventoryByProductAndUserAsync(query.ProductId, query.UserId);
    }
}
