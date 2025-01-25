// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.InfrastructureLayer.Database.SupplierView;

public interface ISupplierViewRepository
{
    Task<DomainModels.SupplierView?> GetSupplierView(long id);

    Task<(List<DomainModels.SupplierView>, string?)> ListSuppliersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent);
}
