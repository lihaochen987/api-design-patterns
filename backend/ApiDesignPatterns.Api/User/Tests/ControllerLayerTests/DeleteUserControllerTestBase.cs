// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using backend.User.ApplicationLayer.Commands.DeleteUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.Controllers;
using Moq;

namespace backend.User.Tests.ControllerLayerTests;

public abstract class DeleteUserControllerTestBase
{
    protected readonly IAsyncQueryHandler<GetUserQuery, DomainModels.User?> MockGetUserHandler =
        Mock.Of<IAsyncQueryHandler<GetUserQuery, DomainModels.User?>>();

    protected readonly ICommandHandler<DeleteUserCommand> MockDeleteUserHandler =
        Mock.Of<ICommandHandler<DeleteUserCommand>>();

    protected DeleteUserController DeleteUserController()
    {
        return new DeleteUserController(MockGetUserHandler, MockDeleteUserHandler);
    }
}
