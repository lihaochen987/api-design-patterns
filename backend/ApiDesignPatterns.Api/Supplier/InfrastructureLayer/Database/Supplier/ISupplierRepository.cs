// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.InfrastructureLayer.Database.Supplier;

public interface ISupplierRepository
{
    Task<DomainModels.Supplier?> GetSupplierAsync(long id);
    Task DeleteSupplierAsync(long id);
    Task CreateSupplierAsync(DomainModels.Supplier supplier);
    Task UpdateSupplierAsync(DomainModels.Supplier supplier);
}
