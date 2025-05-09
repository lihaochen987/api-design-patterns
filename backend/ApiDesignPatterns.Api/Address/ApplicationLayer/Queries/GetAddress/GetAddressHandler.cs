// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.InfrastructureLayer.Database.Address;
using backend.Shared.QueryHandler;

namespace backend.Address.ApplicationLayer.Queries.GetAddress;

public class GetAddressHandler(IAddressRepository repository)
    : IAsyncQueryHandler<GetAddressQuery, DomainModels.Address?>
{
    public async Task<DomainModels.Address?> Handle(GetAddressQuery query)
    {
        var address = await repository.GetAddress(query.Id);
        return address;
    }
}
