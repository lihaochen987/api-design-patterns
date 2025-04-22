// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;

namespace backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;

public class GetSuppliersByIdsHandler (ISupplierViewRepository repository) : IAsyncQueryHandler<GetSuppliersByIdsQuery, List<SupplierView>>
{
    public async Task<List<SupplierView>> Handle(GetSuppliersByIdsQuery query)
    {
        var suppliers = await repository.GetSuppliersByIds(query.SupplierIds);
        return suppliers;
    }
}
