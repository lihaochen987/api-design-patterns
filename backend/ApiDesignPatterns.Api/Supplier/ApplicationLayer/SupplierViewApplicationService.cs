// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer;

namespace backend.Supplier.ApplicationLayer;

public class SupplierViewApplicationService(ISupplierViewRepository repository) : ISupplierViewApplicationService
{
    public async Task<SupplierView?> GetSupplierView(long id)
    {
        SupplierView supplier = await repository.GetSupplierView(id);
        return supplier;
    }
}
