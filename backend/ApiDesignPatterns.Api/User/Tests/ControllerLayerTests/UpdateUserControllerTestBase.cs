// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Commands.UpdateUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.Controllers;
using backend.User.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.User.Tests.ControllerLayerTests;

public abstract class UpdateUserControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IAsyncQueryHandler<GetUserQuery, DomainModels.User?> MockGetUserHandler;
    protected readonly ICommandHandler<UpdateUserCommand> MockUpdateUserHandler;
    protected readonly Fixture Fixture = new();

    protected UpdateUserControllerTestBase()
    {
        MockGetUserHandler = Mock.Of<IAsyncQueryHandler<GetUserQuery, DomainModels.User?>>();
        MockUpdateUserHandler = Mock.Of<ICommandHandler<UpdateUserCommand>>();
        var config = new TypeAdapterConfig();
        config.RegisterUserMappings();
        Mapper = new Mapper(config);
    }

    protected UpdateUserController UpdateUserController()
    {
        return new UpdateUserController(
            MockGetUserHandler,
            MockUpdateUserHandler,
            Mapper);
    }
}
