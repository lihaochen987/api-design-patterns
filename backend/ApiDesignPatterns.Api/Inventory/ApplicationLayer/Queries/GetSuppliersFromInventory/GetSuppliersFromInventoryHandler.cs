// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;

namespace backend.Inventory.ApplicationLayer.Queries.GetSuppliersFromInventory;

public class GetSuppliersFromInventoryHandler : ISyncQueryHandler<GetSuppliersFromInventoryQuery, List<SupplierView?>>
{
    public List<SupplierView?> Handle(GetSuppliersFromInventoryQuery query)
    {
        List<SupplierView?> result = query.Suppliers
            .Where(supplier => supplier != null)
            .ToList();
        return result;
    }
}
