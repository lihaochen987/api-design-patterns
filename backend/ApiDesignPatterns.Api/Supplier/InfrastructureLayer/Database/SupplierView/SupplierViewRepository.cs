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
        var phoneNumbers = await GetSupplierPhoneNumbers(id);

        supplier = supplier with { Addresses = addresses };
        supplier = supplier with { PhoneNumbers = phoneNumbers };

        return supplier;
    }

    public async Task<(List<DomainModels.SupplierView>, string?)> ListSuppliersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        (StringBuilder sql, DynamicParameters parameters) = ApplyPaginationAndFiltering(pageToken, filter, maxPageSize);

        List<DomainModels.SupplierView> suppliers =
            (await dbConnection.QueryAsync<DomainModels.SupplierView>(sql.ToString(), parameters)).ToList();
        List<DomainModels.SupplierView> paginatedSuppliers =
            paginateService.Paginate(suppliers, maxPageSize, out string? nextPageToken);

        if (paginatedSuppliers.Count == 0)
            return (paginatedSuppliers, nextPageToken);

        var supplierIds = paginatedSuppliers.Select(s => s.Id).ToList();

        var addresses = await GetSupplierAddressesByIds(supplierIds);
        var phoneNumbers = await GetSupplierPhoneNumbersByIds(supplierIds);

        var hydratedSuppliers = HydrateSuppliers(paginatedSuppliers, addresses, phoneNumbers);

        return (hydratedSuppliers, nextPageToken);
    }

    private (StringBuilder sql, DynamicParameters parameters) ApplyPaginationAndFiltering(
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

        return (sql, parameters);
    }

    private static List<DomainModels.SupplierView> HydrateSuppliers(
        List<DomainModels.SupplierView> paginatedSuppliers,
        List<AddressWithSupplierId> addresses,
        List<PhoneNumberWithSupplierId> phoneNumbers)
    {
        var addressesBySupplier = addresses
            .GroupBy(a => a.SupplierId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(Address (a) => a).ToList()
            );

        var phoneNumbersBySupplier = phoneNumbers
            .GroupBy(p => p.SupplierId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(PhoneNumber (p) => p).ToList()
            );

        for (int i = 0; i < paginatedSuppliers.Count; i++)
        {
            var supplier = paginatedSuppliers[i];
            paginatedSuppliers[i] = supplier with
            {
                Addresses = addressesBySupplier.TryGetValue(supplier.Id, out var addressList) ? addressList : [],
                PhoneNumbers = phoneNumbersBySupplier.TryGetValue(supplier.Id, out var phoneList) ? phoneList : []
            };
        }

        return paginatedSuppliers;
    }

    private async Task<List<Address>> GetSupplierAddresses(long id)
    {
        var addresses = await dbConnection.QueryAsync<Address>(
            SupplierViewQueries.GetSupplierAddresses,
            new { Id = id });

        return addresses.ToList();
    }

    private async Task<List<PhoneNumber>> GetSupplierPhoneNumbers(long id)
    {
        var phoneNumbers = await dbConnection.QueryAsync<PhoneNumber>(
            SupplierViewQueries.GetSupplierPhoneNumbers,
            new { Id = id });

        return phoneNumbers.ToList();
    }

    private async Task<List<AddressWithSupplierId>> GetSupplierAddressesByIds(List<long> ids)
    {
        var addresses = await dbConnection.QueryAsync<AddressWithSupplierId>(
            SupplierViewQueries.GetSupplierAddressesByIds,
            new { SupplierIds = ids });

        return addresses.ToList();
    }

    private async Task<List<PhoneNumberWithSupplierId>> GetSupplierPhoneNumbersByIds(List<long> ids)
    {
        var phoneNumbers = await dbConnection.QueryAsync<PhoneNumberWithSupplierId>(
            SupplierViewQueries.GetSupplierPhoneNumbersByIds,
            new { SupplierIds = ids });

        return phoneNumbers.ToList();
    }
}
