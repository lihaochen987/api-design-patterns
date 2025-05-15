// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Address.InfrastructureLayer.Database.Address;

public class AddressRepository(IDbConnection dbConnection) : IAddressRepository
{
    public async Task<DomainModels.Address?> GetAddress(long id)
    {
        var address =
            await dbConnection.QueryFirstOrDefaultAsync<DomainModels.Address>(AddressQueries.GetAddress,
                new { Id = id });
        return address;
    }

    public async Task DeleteAddress(long id)
    {
        await dbConnection.ExecuteAsync(AddressQueries.DeleteAddress, new { Id = id });
    }

    public async Task UpdateAddressAsync(DomainModels.Address address)
    {
        await dbConnection.ExecuteAsync(AddressQueries.UpdateAddress,
            new
            {
                address.Id,
                address.SupplierId,
                address.Street,
                address.City,
                address.PostalCode,
                address.Country,
            });
    }
}
