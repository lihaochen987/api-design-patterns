// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels;
using backend.Address.InfrastructureLayer.Database.AddressView;
using backend.Shared.QueryHandler;

namespace backend.Address.ApplicationLayer.Queries.ListAddress;

public class ListAddressHandler(IListAddressView repository) : IAsyncQueryHandler<ListAddressQuery, PagedAddress>
{
    public async Task<PagedAddress> Handle(ListAddressQuery query)
    {
        (List<AddressView> address, string? nextPageToken) = await repository.ListAddressAsync(
            query.PageToken,
            query.Filter,
            query.MaxPageSize);
        return new PagedAddress(address, nextPageToken);
    }
}
