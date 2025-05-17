// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;
using backend.Shared;
using backend.Shared.QueryHandler;
using backend.User.DomainModels;
using backend.User.Tests.TestHelpers.Fakes;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetUsersByIdsHandlerTestBase
{
    protected readonly UserViewRepositoryFake Repository = new(new PaginateService<UserView>());
    protected readonly Fixture Fixture = new();

    protected IAsyncQueryHandler<GetUsersByIdsQuery, List<UserView>> GetUsersByIdsHandler()
    {
        return new GetUsersByIdsHandler(Repository);
    }
}
