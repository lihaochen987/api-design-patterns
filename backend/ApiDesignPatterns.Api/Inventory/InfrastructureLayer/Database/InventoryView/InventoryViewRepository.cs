// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Shared;
using Dapper;

namespace backend.Inventory.InfrastructureLayer.Database.InventoryView;

public class InventoryViewRepository(
    IDbConnection dbConnection,
    SqlFilterBuilder inventorySqlFilterBuilder,
    PaginateService<DomainModels.InventoryView> paginateService)
    : IInventoryViewRepository
{
    public async Task<DomainModels.InventoryView?> GetInventoryView(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.InventoryView>(
            InventoryViewQueries.GetInventoryView,
            new { Id = id });
    }

    public async Task<(List<DomainModels.InventoryView>, string?)> ListInventoryAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var sql = new StringBuilder(InventoryViewQueries.ListInventoryBase);
        var parameters = new DynamicParameters();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenInventory))
        {
            sql.Append(" AND inventory_id > @LastSeenInventory");
            parameters.Add("LastSeenInventory", lastSeenInventory);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = inventorySqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY inventory_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        List<DomainModels.InventoryView> inventory =
            (await dbConnection.QueryAsync<DomainModels.InventoryView>(sql.ToString(), parameters)).ToList();
        List<DomainModels.InventoryView> paginatedInventory =
            paginateService.Paginate(inventory, maxPageSize, out string? nextPageToken);

        return (paginatedInventory, nextPageToken);
    }
}
