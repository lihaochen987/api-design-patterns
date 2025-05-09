// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels;
using backend.Address.InfrastructureLayer.Database.AddressView;
using backend.Shared.QueryHandler;

namespace backend.Address.ApplicationLayer.Queries.GetAddressView;

public class GetAddressViewHandler(IAddressViewRepository viewRepository)
    : IAsyncQueryHandler<GetAddressViewQuery, AddressView?>
{
    public async Task<AddressView?> Handle(GetAddressViewQuery query)
    {
        var addressView = await viewRepository.GetAddressViewAsync(query.Id);
        return addressView;
    }
}
