// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;

namespace backend.Supplier.ApplicationLayer.Queries.ListSuppliers;

public class ListSuppliersHandler(ISupplierViewRepository repository)
    : IQueryHandler<ListSuppliersQuery, PagedSuppliers>
{
    public async Task<PagedSuppliers> Handle(ListSuppliersQuery query)
    {
        (List<SupplierView> suppliers, string? nextPageToken) = await repository.ListSuppliersAsync(
            query.Request.PageToken,
            query.Request.Filter,
            query.Request.MaxPageSize);
        return new PagedSuppliers(suppliers, nextPageToken);
    }
}
