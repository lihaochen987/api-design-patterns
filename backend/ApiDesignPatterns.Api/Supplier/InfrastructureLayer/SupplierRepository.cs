// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.InfrastructureLayer.Database;

namespace backend.Supplier.InfrastructureLayer;

public class SupplierRepository(SupplierDbContext context) : ISupplierRepository
{
    public async Task<DomainModels.Supplier?> GetSupplierAsync(long id) => await context.Suppliers.FindAsync(id);

    public async Task DeleteSupplierAsync(DomainModels.Supplier supplier)
    {
        context.Suppliers.Remove(supplier);
        await context.SaveChangesAsync();
    }

    public async Task CreateSupplierAsync(DomainModels.Supplier supplier)
    {
        context.Suppliers.Add(supplier);
        await context.SaveChangesAsync();
    }
}
