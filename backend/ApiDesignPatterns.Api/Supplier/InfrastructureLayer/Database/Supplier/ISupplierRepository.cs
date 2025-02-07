// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.InfrastructureLayer.Database.Supplier;

public interface ISupplierRepository
{
    Task<DomainModels.Supplier?> GetSupplierAsync(long id);
    Task DeleteSupplierAsync(long id);
    Task<long> CreateSupplierAsync(DomainModels.Supplier supplier);
    Task<long> UpdateSupplierAsync(DomainModels.Supplier supplier);
    Task CreateSupplierAddressAsync(DomainModels.Supplier supplier);
    Task CreateSupplierPhoneNumberAsync(DomainModels.Supplier supplier);
    Task UpdateSupplierPhoneNumberAsync(DomainModels.Supplier supplier);
    Task UpdateSupplierAddressAsync(DomainModels.Supplier supplier);
}
