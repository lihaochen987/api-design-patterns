// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Queries.ListUsers;
using backend.User.DomainModels;
using backend.User.Tests.TestHelpers.Fakes;

namespace backend.User.Tests.ApplicationLayerTests;

public abstract class ListUsersHandlerTestBase
{
    protected readonly UserViewRepositoryFake Repository = new(new PaginateService<UserView>());

    protected IAsyncQueryHandler<ListUsersQuery, PagedUsers> ListUsersViewHandler()
    {
        return new ListUsersHandler(Repository);
    }
}
