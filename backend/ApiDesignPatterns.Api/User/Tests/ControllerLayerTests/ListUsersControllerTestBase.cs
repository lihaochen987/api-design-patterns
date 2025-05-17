// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Queries.ListUsers;
using backend.User.Controllers;
using backend.User.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.User.Tests.ControllerLayerTests;

public abstract class ListUsersControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListUsersQuery, PagedUsers> MockListUsers;
    protected readonly IMapper Mapper;
    protected const int DefaultMaxPageSize = 10;

    protected ListUsersControllerTestBase()
    {
        MockListUsers = Mock.Of<IAsyncQueryHandler<ListUsersQuery, PagedUsers>>();
        var config = new TypeAdapterConfig();
        config.RegisterUserMappings();
        Mapper = new Mapper(config);
    }

    protected ListUsersController ListUsersController()
    {
        return new ListUsersController(MockListUsers, Mapper);
    }
}
