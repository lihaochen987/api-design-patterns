// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;

public class GetInventoryByProductAndSupplierHandler(IInventoryRepository repository)
    : IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?>
{
    public Task<DomainModels.Inventory?> Handle(GetInventoryByProductAndSupplierQuery query)
    {
        return repository.GetInventoryByProductAndSupplierAsync(query.ProductId, query.SupplierId);
    }
}
