// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Shared;
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

        var phoneNumbers = await GetPhoneNumberIds(id);
        var addresses = await GetAddressIds(id);

        supplier = supplier with { PhoneNumberIds = phoneNumbers, AddressIds = addresses };

        return supplier;
    }

    private async Task<List<long>> GetPhoneNumberIds(long supplierId)
    {
        var results = await dbConnection.QueryAsync<long>(
            SupplierViewQueries.GetPhoneNumbers,
            new { Id = supplierId });

        return results.Select(r => r).ToList();
    }

    private async Task<List<long>> GetAddressIds(long supplierId)
    {
        var results = await dbConnection.QueryAsync<long>(
            SupplierViewQueries.GetAddresses,
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

        var supplierIds = paginatedSuppliers.Select(s => s.Id).ToList();
        var phoneNumbersBySupplier = await GetPhoneNumberIdsForSuppliers(supplierIds);
        var addressesBySupplier = await GetAddressIdsForSuppliers(supplierIds);

        paginatedSuppliers = paginatedSuppliers
            .Select(supplier => supplier with
            {
                PhoneNumberIds = phoneNumbersBySupplier.TryGetValue(supplier.Id, out var phoneNumberIds)
                    ? phoneNumberIds
                    : [],
                AddressIds = addressesBySupplier.TryGetValue(supplier.Id, out var addressIds)
                    ? addressIds
                    : [],
            })
            .ToList();

        return (paginatedSuppliers, nextPageToken);
    }


    private async Task<Dictionary<long, List<long>>> GetPhoneNumberIdsForSuppliers(List<long> supplierIds)
    {
        if (supplierIds.Count == 0)
            return new Dictionary<long, List<long>>();

        var results = await dbConnection.QueryAsync<(long SupplierId, long PhoneNumberId)>(
            SupplierViewQueries.GetPhoneNumbersForMultipleSuppliers,
            new { SupplierIds = supplierIds });

        return results
            .GroupBy(r => r.SupplierId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => r.PhoneNumberId).ToList());
    }

    private async Task<Dictionary<long, List<long>>> GetAddressIdsForSuppliers(List<long> supplierIds)
    {
        if (supplierIds.Count == 0)
            return new Dictionary<long, List<long>>();

        var results = await dbConnection.QueryAsync<(long SupplierId, long AddressId)>(
            SupplierViewQueries.GetAddressesForMultipleSuppliers,
            new { SupplierIds = supplierIds });

        return results
            .GroupBy(r => r.SupplierId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => r.AddressId).ToList());
    }
}
