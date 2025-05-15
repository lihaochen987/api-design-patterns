// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.ApplicationLayer.Commands.ReplaceAddress;
using backend.Address.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Address.Tests.ApplicationLayerTests;

public abstract class ReplaceAddressHandlerTestBase
{
    protected readonly AddressRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected ICommandHandler<ReplaceAddressCommand> ReplaceAddressHandler()
    {
        return new ReplaceAddressHandler(Repository);
    }
}
