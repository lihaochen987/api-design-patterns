// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Shared;
using Dapper;

namespace backend.Address.InfrastructureLayer.Database.AddressView;

public class AddressViewRepository(
    IDbConnection dbConnection,
    SqlFilterBuilder addressSqlFilterBuilder,
    PaginateService<DomainModels.AddressView> paginateService) : IGetAddressView, IListAddressView
{
    public async Task<DomainModels.AddressView?> GetAddressViewAsync(long id)
    {
        var addressViewQueries = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.AddressView>(
            AddressViewQueries.GetAddressView, new { Id = id });

        return addressViewQueries;
    }

    public async Task<(List<DomainModels.AddressView>, string?)> ListAddressAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var sql = new StringBuilder(AddressViewQueries.ListAddressBase);
        var parameters = new DynamicParameters();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenAddress))
        {
            sql.Append(" AND address_id > @LastSeenAddress");
            parameters.Add("LastSeenAddress", lastSeenAddress);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = addressSqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY address_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        var address =
            (await dbConnection.QueryAsync<DomainModels.AddressView>(sql.ToString(), parameters)).ToList();
        var paginatedAddress =
            paginateService.Paginate(address, maxPageSize, out string? nextPageToken);

        return (paginatedAddress, nextPageToken);
    }
}
