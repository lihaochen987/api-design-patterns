// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.UpdateUser;
using backend.User.Tests.TestHelpers.Fakes;

namespace backend.User.Tests.ApplicationLayerTests;

public abstract class UpdateUserHandlerTestBase
{
    protected readonly UserRepositoryFake Repository = [];

    protected ICommandHandler<UpdateUserCommand> UpdateUserHandler()
    {
        return new UpdateUserHandler(Repository);
    }
}
