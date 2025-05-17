// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.Tests.TestHelpers.Fakes;

namespace backend.User.Tests.ApplicationLayerTests;

public abstract class GetUserHandlerTestBase
{
    protected readonly UserRepositoryFake Repository = [];

    protected IAsyncQueryHandler<GetUserQuery, DomainModels.User?> GetUserHandler()
    {
        return new GetUserHandler(Repository);
    }
}
