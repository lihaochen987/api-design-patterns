// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using backend.Shared;
using Dapper;
using SqlFilterBuilder = backend.Shared.SqlFilterBuilder;

namespace backend.User.InfrastructureLayer.Database.UserView;

public class UserViewRepository(
    IDbConnection dbConnection,
    SqlFilterBuilder sqlFilterBuilder,
    PaginateService<DomainModels.UserView> paginateService)
    : IUserViewRepository
{
    public async Task<DomainModels.UserView?> GetUserView(long id)
    {
        var user = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.UserView>(
            UserViewQueries.GetUserView,
            new { Id = id });

        if (user == null)
            return null;

        var phoneNumbers = await GetPhoneNumberIds(id);
        var addresses = await GetAddressIds(id);

        user = user with { PhoneNumberIds = phoneNumbers, AddressIds = addresses };

        return user;
    }

    private async Task<List<long>> GetPhoneNumberIds(long userId)
    {
        var results = await dbConnection.QueryAsync<long>(
            UserViewQueries.GetPhoneNumbers,
            new { Id = userId });

        return results.Select(r => r).ToList();
    }

    private async Task<List<long>> GetAddressIds(long userId)
    {
        var results = await dbConnection.QueryAsync<long>(
            UserViewQueries.GetAddresses,
            new { Id = userId });

        return results.Select(r => r).ToList();
    }

    public async Task<(List<DomainModels.UserView>, string?)> ListUsersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var sql = new StringBuilder(UserViewQueries.ListUsersBase);
        var parameters = new DynamicParameters();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenUser))
        {
            sql.Append(" AND user_id > @LastSeenUser");
            parameters.Add("LastSeenUser", lastSeenUser);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            string filterClause = sqlFilterBuilder.BuildSqlWhereClause(filter);
            sql.Append($" AND {filterClause}");
        }

        // Order and limit
        sql.Append(" ORDER BY user_id LIMIT @PageSizePlusOne");
        parameters.Add("PageSizePlusOne", maxPageSize + 1);

        List<DomainModels.UserView> users =
            (await dbConnection.QueryAsync<DomainModels.UserView>(sql.ToString(), parameters)).ToList();
        List<DomainModels.UserView> paginatedUsers =
            paginateService.Paginate(users, maxPageSize, out string? nextPageToken);

        var userIds = paginatedUsers.Select(s => s.Id).ToList();
        var phoneNumbersByUser = await GetPhoneNumberIdsForUsers(userIds);
        var addressesByUser = await GetAddressIdsForUsers(userIds);

        paginatedUsers = paginatedUsers
            .Select(user => user with
            {
                PhoneNumberIds = phoneNumbersByUser.TryGetValue(user.Id, out var phoneNumberIds)
                    ? phoneNumberIds
                    : [],
                AddressIds = addressesByUser.TryGetValue(user.Id, out var addressIds)
                    ? addressIds
                    : [],
            })
            .ToList();

        return (paginatedUsers, nextPageToken);
    }


    private async Task<Dictionary<long, List<long>>> GetPhoneNumberIdsForUsers(List<long> userIds)
    {
        if (userIds.Count == 0)
            return new Dictionary<long, List<long>>();

        var results = await dbConnection.QueryAsync<(long UserId, long PhoneNumberId)>(
            UserViewQueries.GetPhoneNumbersForMultipleUsers,
            new { UserIds = userIds });

        return results
            .GroupBy(r => r.UserId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => r.PhoneNumberId).ToList());
    }

    private async Task<Dictionary<long, List<long>>> GetAddressIdsForUsers(List<long> userIds)
    {
        if (userIds.Count == 0)
            return new Dictionary<long, List<long>>();

        var results = await dbConnection.QueryAsync<(long UserId, long AddressId)>(
            UserViewQueries.GetAddressesForMultipleUsers,
            new { UserIds = userIds });

        return results
            .GroupBy(r => r.UserId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => r.AddressId).ToList());
    }
}
