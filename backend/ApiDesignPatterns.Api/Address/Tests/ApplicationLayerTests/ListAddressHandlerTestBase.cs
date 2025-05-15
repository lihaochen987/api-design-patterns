// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.ApplicationLayer.Queries.ListAddress;
using backend.Address.DomainModels;
using backend.Address.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Address.Tests.ApplicationLayerTests;

public abstract class ListAddressHandlerTestBase
{
    protected readonly AddressViewRepositoryFake Repository = new(new PaginateService<AddressView>());
    protected readonly Fixture Fixture = new();

    protected IAsyncQueryHandler<ListAddressQuery, PagedAddress> ListAddressHandler()
    {
        return new ListAddressHandler(Repository);
    }
}
