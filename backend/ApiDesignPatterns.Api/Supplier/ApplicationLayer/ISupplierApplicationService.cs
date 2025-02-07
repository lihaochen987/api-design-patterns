// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.SupplierControllers;

namespace backend.Supplier.ApplicationLayer;

public interface ISupplierApplicationService
{
    Task<DomainModels.Supplier?> GetSupplierAsync(long id);

    Task DeleteSupplierAsync(long id);
    Task ReplaceSupplierAsync(DomainModels.Supplier supplier, long id);
    Task UpdateSupplierAsync(UpdateSupplierRequest request, DomainModels.Supplier supplier, long id);
}
