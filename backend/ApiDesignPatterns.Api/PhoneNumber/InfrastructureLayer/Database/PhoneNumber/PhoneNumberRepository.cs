// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumber;

public class PhoneNumberRepository(IDbConnection dbConnection) : IPhoneNumberRepository
{
    public async Task<DomainModels.PhoneNumber?> GetPhoneNumber(long id)
    {
        var phoneNumber = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.PhoneNumber>(
            PhoneNumberQueries.GetPhoneNumber,
            new { Id = id });

        return phoneNumber;
    }

    public async Task DeletePhoneNumber(long id)
    {
        await dbConnection.ExecuteAsync(PhoneNumberQueries.DeletePhoneNumber, new { Id = id });
    }
}
