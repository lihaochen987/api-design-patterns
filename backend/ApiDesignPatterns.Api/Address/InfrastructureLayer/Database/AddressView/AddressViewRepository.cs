// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Address.InfrastructureLayer.Database.AddressView;

public class AddressViewRepository(IDbConnection dbConnection) : IAddressViewRepository
{
    public async Task<DomainModels.AddressView?> GetAddressViewAsync(long id)
    {
        var addressViewQueries = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.AddressView>(
            AddressViewQueries.GetAddressView, new { Id = id });

        return addressViewQueries;
    }
}
