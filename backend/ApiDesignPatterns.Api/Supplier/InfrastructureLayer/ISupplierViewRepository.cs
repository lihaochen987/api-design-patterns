// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;

namespace backend.Supplier.InfrastructureLayer;

public interface ISupplierViewRepository
{
    Task<SupplierView?> GetSupplierView(long id);

    Task<(List<SupplierView>, string?)> ListSuppliersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent);
}
