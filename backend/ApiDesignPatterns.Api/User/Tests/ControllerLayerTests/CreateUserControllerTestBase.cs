// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.CreateUser;
using backend.User.Controllers;
using backend.User.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.User.Tests.ControllerLayerTests;

public abstract class CreateUserControllerTestBase
{
    protected readonly ICommandHandler<CreateUserCommand> CreateUser =
        Mock.Of<ICommandHandler<CreateUserCommand>>();

    protected readonly IMapper Mapper;

    protected Fixture Fixture = new();

    protected CreateUserControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterUserMappings();
        Mapper = new Mapper(config);
    }

    protected CreateUserController GetCreateUserController()
    {
        return new CreateUserController(
            CreateUser,
            Mapper);
    }
}
