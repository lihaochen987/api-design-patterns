// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.InfrastructureLayer;

namespace backend.Supplier.ApplicationLayer;

public class SupplierApplicationService(ISupplierRepository repository) : ISupplierApplicationService
{
    public async Task<DomainModels.Supplier?> GetSupplierAsync(long id)
    {
        DomainModels.Supplier? supplier = await repository.GetSupplierAsync(id);

        return supplier ?? null;
    }

    public async Task DeleteSupplierAsync(DomainModels.Supplier supplier) =>
        await repository.DeleteSupplierAsync(supplier);
}
