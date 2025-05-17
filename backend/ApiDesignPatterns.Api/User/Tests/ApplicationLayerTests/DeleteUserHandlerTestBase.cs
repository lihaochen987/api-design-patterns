// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.DeleteUser;
using backend.User.Tests.TestHelpers.Fakes;

namespace backend.User.Tests.ApplicationLayerTests;

public abstract class DeleteUserHandlerTestBase
{
    protected readonly UserRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected ICommandHandler<DeleteUserCommand> DeleteUserService()
    {
        return new DeleteUserHandler(Repository);
    }
}
