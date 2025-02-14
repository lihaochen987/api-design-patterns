// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Shared;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;
using backend.Supplier.Tests.TestHelpers.Builders;

namespace backend.Supplier.Tests.TestHelpers.Fakes;

public class SupplierViewRepositoryFake(
    QueryService<SupplierView> queryService)
    : Collection<SupplierView>, ISupplierViewRepository
{
    public void AddSupplierView(string firstName, string lastName, string email)
    {
        var supplierView = new SupplierViewTestDataBuilder()
            .WithFullName(firstName + " " + lastName)
            .WithEmail(email)
            .Build();
        Add(supplierView);
    }

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
        var query = this.AsEnumerable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenSupplier))
        {
            query = query.Where(s => s.Id > lastSeenSupplier);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            if (filter.Contains("Email.endsWith"))
            {
                string value = filter.Split('"')[1];
                query = query.Where(s => s.Email.EndsWith(value));
            }
            else if (filter.Contains("FullName =="))
            {
                string value = filter.Split('"')[1];
                query = query.Where(s => s.FullName == value);
            }
        }

        var suppliers = query
            .OrderBy(s => s.Id)
            .Take(maxPageSize + 1)
            .ToList();

        List<SupplierView> paginatedSuppliers =
            queryService.Paginate(suppliers, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedSuppliers, nextPageToken));
    }
}
