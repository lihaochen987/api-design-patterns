// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Commands.ReplaceUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.Controllers;
using backend.User.Services;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.User.Tests.ControllerLayerTests;

public abstract class ReplaceUserControllerTestBase
{
    protected readonly IAsyncQueryHandler<GetUserQuery, DomainModels.User?> GetUser =
        Mock.Of<IAsyncQueryHandler<GetUserQuery, DomainModels.User?>>();

    protected readonly ICommandHandler<ReplaceUserCommand> ReplaceUser =
        Mock.Of<ICommandHandler<ReplaceUserCommand>>();

    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ReplaceUserControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterUserMappings();
        Mapper = new Mapper(config);
    }

    protected ReplaceUserController GetReplaceUserController()
    {
        return new ReplaceUserController(
            GetUser,
            ReplaceUser,
            Mapper);
    }
}
