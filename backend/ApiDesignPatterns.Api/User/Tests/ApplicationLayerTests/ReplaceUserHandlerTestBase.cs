// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.ReplaceUser;
using backend.User.Tests.TestHelpers.Fakes;

namespace backend.User.Tests.ApplicationLayerTests;

public abstract class ReplaceUserHandlerTestBase
{
    protected readonly UserRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected ICommandHandler<ReplaceUserCommand> ReplaceUserHandler()
    {
        return new ReplaceUserHandler(Repository);
    }
}
