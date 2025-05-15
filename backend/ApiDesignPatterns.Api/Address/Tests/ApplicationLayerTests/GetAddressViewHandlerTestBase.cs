// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.GetAddressView;
using backend.Address.DomainModels;
using backend.Address.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Address.Tests.ApplicationLayerTests;

public abstract class GetAddressViewHandlerTestBase
{
    protected readonly AddressViewRepositoryFake Repository = [];

    protected IAsyncQueryHandler<GetAddressViewQuery, AddressView?> GetAddressViewHandler()
    {
        return new GetAddressViewHandler(Repository);
    }
}
