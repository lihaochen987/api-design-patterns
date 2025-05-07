// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Shared;
using backend.Supplier.DomainModels.ValueObjects;
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
        await using var multi = await dbConnection.QueryMultipleAsync(
            $"{SupplierViewQueries.GetSupplierView} {SupplierViewQueries.GetSupplierAddresses} {SupplierViewQueries.GetSupplierPhoneNumbers}",
            new { Id = id });

        var supplier = await multi.ReadSingleOrDefaultAsync<DomainModels.SupplierView>();

        if (supplier == null)
            return null;

        var addresses = await multi.ReadAsync<Address>();
        var phoneNumbers = await multi.ReadAsync<PhoneNumber>();

        supplier = supplier with { Addresses = addresses.ToList() };
        supplier = supplier with { PhoneNumbers = phoneNumbers.ToList() };

        return supplier;
    }

    public async Task<List<DomainModels.SupplierView>> GetSuppliersByIds(List<long> supplierIds)
    {
        if (supplierIds.Count == 0)
        {
            return [];
        }

        var parameters = new DynamicParameters();
        parameters.Add("@SupplierIds", supplierIds);
        var results = await dbConnection.QueryAsync<DomainModels.SupplierView>(SupplierViewQueries.GetSuppliersByIds,
            new { SupplierIds = supplierIds });
        return results.ToList();
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
}
