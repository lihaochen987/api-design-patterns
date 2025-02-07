// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.InfrastructureLayer.Database.Supplier;

namespace backend.Supplier.ApplicationLayer.Queries.GetSupplier;

public class GetSupplierHandler(ISupplierRepository repository) : IQueryHandler<GetSupplierQuery, DomainModels.Supplier>
{
    public async Task<DomainModels.Supplier?> Handle(GetSupplierQuery query)
    {
        DomainModels.Supplier? supplier = await repository.GetSupplierAsync(query.Id);
        return supplier;
    }
}
