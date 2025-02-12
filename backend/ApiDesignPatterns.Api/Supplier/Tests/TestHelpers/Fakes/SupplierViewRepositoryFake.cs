// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Linq.Expressions;
using backend.Shared;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;

namespace backend.Supplier.Tests.TestHelpers.Fakes;

public class SupplierViewRepositoryFake(
    QueryService<SupplierView> queryService)
    : Collection<SupplierView>, ISupplierViewRepository
{
    public Task<SupplierView?> GetSupplierView(long id)
    {
        SupplierView? supplierView = this.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(supplierView);
    }

    public Task<(List<SupplierView>, string?)> ListSuppliersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        IEnumerable<SupplierView> query = this.AsEnumerable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenSupplierId))
        {
            query = query.Where(s => s.Id > lastSeenSupplierId);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            Expression<Func<SupplierView, bool>> filterExpression = queryService.BuildFilterExpression(filter);
            query = query.Where(filterExpression.Compile());
        }

        List<SupplierView> suppliers = query.OrderBy(s => s.Id).ToList();

        List<SupplierView> paginatedSuppliers =
            queryService.Paginate(suppliers, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedSuppliers, nextPageToken));
    }
}
