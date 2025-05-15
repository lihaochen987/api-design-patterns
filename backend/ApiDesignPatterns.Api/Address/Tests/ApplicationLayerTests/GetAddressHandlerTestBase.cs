// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Queries.GetAddress;
using backend.Address.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Address.Tests.ApplicationLayerTests;

public abstract class GetAddressHandlerTestBase
{
    protected readonly AddressRepositoryFake Repository = [];

    protected IAsyncQueryHandler<GetAddressQuery, DomainModels.Address?> GetAddressHandler()
    {
        return new GetAddressHandler(Repository);
    }
}
