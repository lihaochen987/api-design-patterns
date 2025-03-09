// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;

namespace backend.Supplier.ApplicationLayer.Queries.GetSupplierView;

public class GetSupplierViewHandler(
    ISupplierViewRepository repository)
    : IQueryHandler<GetSupplierViewQuery, SupplierView?>
{
    public async Task<SupplierView?> Handle(GetSupplierViewQuery query)
    {
        SupplierView? supplier = await repository.GetSupplierView(query.Id);
        return supplier;
    }
}
