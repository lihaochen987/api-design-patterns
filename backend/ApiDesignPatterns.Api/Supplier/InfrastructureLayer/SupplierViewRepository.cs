// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Queries;
using backend.Supplier.Services;
using Dapper;

namespace backend.Supplier.InfrastructureLayer;

public class SupplierViewRepository(
    IDbConnection dbConnection,
    ProductSqlFilterBuilder sqlFilterBuilder)
    : ISupplierViewRepository
{
    public async Task<SupplierView?> GetSupplierView(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<SupplierView>(SupplierViewQueries.GetSupplierView,
            new { Id = id });
    }

    public async Task<(List<SupplierView>, string?)> ListSuppliersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent)
    {
        var sql = new StringBuilder(SupplierViewQueries.ListSuppliersBase);
        var parameters = new DynamicParameters();

        // Parent filter
        if (!string.IsNullOrWhiteSpace(parent) && long.TryParse(parent, out long parentId))
        {
            sql.Append(" AND product_id = @ParentId");
            parameters.Add("ParentId", parentId);
        }

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

        var suppliers = (await dbConnection.QueryAsync<SupplierView>(sql.ToString(), parameters)).ToList();
        var paginatedSuppliers = Paginate(suppliers, maxPageSize, out string? nextPageToken);

        return (paginatedSuppliers, nextPageToken);
    }

    private static List<SupplierView> Paginate(
        List<SupplierView> existingItems,
        int maxPageSize,
        out string? nextPageToken)
    {
        if (existingItems.Count <= maxPageSize)
        {
            nextPageToken = null;
            return existingItems;
        }

        SupplierView lastItemInPage = existingItems[maxPageSize - 1];
        nextPageToken = lastItemInPage.Id.ToString();
        return existingItems.Take(maxPageSize).ToList();
    }
}
