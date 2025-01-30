// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;
using backend.Supplier.SupplierControllers;

namespace backend.Supplier.ApplicationLayer;

public class SupplierViewApplicationService(ISupplierViewRepository repository) : ISupplierViewApplicationService
{
    public async Task<SupplierView?> GetSupplierView(long id)
    {
        SupplierView? supplier = await repository.GetSupplierView(id);
        return supplier;
    }

    public async Task<(List<SupplierView>, string?)> ListSuppliersAsync(ListSuppliersRequest request)
    {
        (List<SupplierView> suppliers, string? nextPageToken) = await repository.ListSuppliersAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);
        return (suppliers, nextPageToken);
    }
}
