// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumberView;

public class PhoneNumberViewRepository(IDbConnection dbConnection) : IPhoneNumberViewRepository
{
    public async Task<DomainModels.PhoneNumberView?> GetPhoneNumberViewAsync(long id)
    {
        var phoneNumber = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.PhoneNumberView>(
            PhoneNumberViewQueries.GetPhoneNumberView,
            new { Id = id });

        return phoneNumber;
    }
}
