// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Shared;
using backend.Supplier.DomainModels.ValueObjects;
using backend.Supplier.InfrastructureLayer.Database.Mapping;
using Dapper;
using SqlFilterBuilder = backend.Shared.SqlFilterBuilder;

namespace backend.Supplier.InfrastructureLayer.Database.SupplierView;

public class SupplierViewRepository(
    IDbConnection dbConnection,
    SqlFilterBuilder sqlFilterBuilder,
    PaginateService<DomainModels.SupplierView> paginateService)
    : ISupplierViewRepository
{
    public async Task<DomainModels.SupplierView?> GetSupplierView(long id)
    {
        var supplier = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.SupplierView>(
            SupplierViewQueries.GetSupplierView,
            new { Id = id });

        if (supplier == null)
            return null;

        var addresses = await GetSupplierAddresses(id);
        var phoneNumbers = await GetPhoneNumberIds(id);

        supplier = supplier with { Addresses = addresses, PhoneNumberIds = phoneNumbers };

        return supplier;
    }

    private async Task<List<long>> GetPhoneNumberIds(long supplierId)
    {
        var results = await dbConnection.QueryAsync<long>(
            SupplierViewQueries.GetPhoneNumbers,
            new { Id = supplierId });

        return results.Select(r => r).ToList();
    }

    public async Task<(List<DomainModels.SupplierView>, string?)> ListSuppliersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var sql = new StringBuilder(SupplierViewQueries.ListSuppliersBase);
        var parameters = new DynamicParameters();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenSupplier))
        {
            sql.Append(" AND supplier_id > @LastSeenSupplier");
            parameters.Add("LastSeenSupplier", lastSeenSupplier);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = sqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY supplier_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        List<DomainModels.SupplierView> suppliers =
            (await dbConnection.QueryAsync<DomainModels.SupplierView>(sql.ToString(), parameters)).ToList();
        List<DomainModels.SupplierView> paginatedSuppliers =
            paginateService.Paginate(suppliers, maxPageSize, out string? nextPageToken);

        return (paginatedSuppliers, nextPageToken);
    }

    private async Task<List<Address>> GetSupplierAddresses(long id)
    {
        var addresses = await dbConnection.QueryAsync<Address>(
            SupplierViewQueries.GetSupplierAddresses,
            new { Id = id });

        return addresses.ToList();
    }
}
