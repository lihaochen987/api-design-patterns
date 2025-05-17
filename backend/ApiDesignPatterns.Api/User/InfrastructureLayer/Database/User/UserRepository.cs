// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.User.InfrastructureLayer.Database.User;

public class UserRepository(
    IDbConnection dbConnection)
    : IUserRepository
{
    public async Task<DomainModels.User?> GetUserAsync(long id)
    {
        var user = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.User>(UserQueries.GetUser,
            new { Id = id });
        if (user == null)
        {
            return null;
        }

        var phoneNumberIds = await GetUserPhoneNumberIds(id);
        var addressIds = await GetUserAddressIds(id);
        var hydratedUser = user with { PhoneNumberIds = phoneNumberIds, AddressIds = addressIds };
        return hydratedUser;
    }

    public async Task DeleteUserAsync(long id)
    {
        await dbConnection.ExecuteAsync(UserQueries.DeleteUser, new { Id = id });
    }

    public async Task<long> CreateUserAsync(DomainModels.User user)
    {
        const string insertUserQuery = UserQueries.CreateUser;
        return await dbConnection.ExecuteScalarAsync<long>(
            insertUserQuery,
            new { user.FirstName, user.LastName, user.Email, user.CreatedAt }
        );
    }

    public async Task<long> UpdateUserAsync(DomainModels.User newUser, DomainModels.User oldUser)
    {
        const string updateUserQuery = UserQueries.UpdateUser;
        long userId = await dbConnection.ExecuteScalarAsync<long>(
            updateUserQuery,
            new { newUser.Id, newUser.FirstName, newUser.LastName, newUser.Email }
        );
        await UpdateUserPhoneNumberIds(newUser.PhoneNumberIds, oldUser.PhoneNumberIds, userId);
        await UpdateUserAddressIds(newUser.AddressIds, oldUser.AddressIds, userId);

        return userId;
    }

    public async Task<long> ReplaceUserAsync(DomainModels.User user)
    {
        const string updateUserQuery = UserQueries.UpdateUser;
        long userId = await dbConnection.ExecuteScalarAsync<long>(
            updateUserQuery,
            new { user.Id, user.FirstName, user.LastName, user.Email }
        );
        return userId;
    }

    private async Task UpdateUserPhoneNumberIds(
        ICollection<long> newPhoneNumberIds,
        ICollection<long> oldPhoneNumberIds,
        long userId)
    {
        await dbConnection.ExecuteAsync(
            UserQueries.UpdateOldUserPhoneNumberId,
            new { PhoneNumberIds = oldPhoneNumberIds }
        );

        const string updatePhoneNumberQuery = UserQueries.UpdateUserPhoneNumberId;
        var phoneParameters = newPhoneNumberIds.Select(phoneNumberId => new
        {
            PhoneNumberId = phoneNumberId, UserId = userId,
        }).ToList();

        await dbConnection.ExecuteAsync(updatePhoneNumberQuery, phoneParameters);
    }

    private async Task UpdateUserAddressIds(
        ICollection<long> newAddressIds,
        ICollection<long> oldAddressIds,
        long userId)
    {
        await dbConnection.ExecuteAsync(
            UserQueries.UpdateOldUserAddressId,
            new { AddressIds = oldAddressIds }
        );

        const string updateAddressQuery = UserQueries.UpdateUserAddressId;
        var addressParameters =
            newAddressIds.Select(addressId => new { AddressId = addressId, UserId = userId, }).ToList();

        await dbConnection.ExecuteAsync(updateAddressQuery, addressParameters);
    }

    private async Task<List<long>> GetUserPhoneNumberIds(long userId)
    {
        var phoneNumberIds = await dbConnection.QueryAsync<long>(
            UserQueries.GetUserPhoneNumberIds,
            new { UserId = userId });
        return phoneNumberIds.ToList();
    }

    private async Task<List<long>> GetUserAddressIds(long userId)
    {
        var addressIds = await dbConnection.QueryAsync<long>(
            UserQueries.GetUserAddressIds,
            new { UserId = userId });
        return addressIds.ToList();
    }
}
