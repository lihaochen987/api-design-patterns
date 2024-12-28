// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace backend.Supplier.InfrastructureLayer;

public class SupplierViewRepository(SupplierDbContext context) : ISupplierViewRepository
{
    public async Task<SupplierView?> GetSupplierView(long id) =>
        await context.Set<SupplierView>()
            .FirstOrDefaultAsync(p => p.Id == id);
}
