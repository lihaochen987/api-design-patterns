// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Commands.UpdateAddress;
using backend.Address.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Address.Tests.ApplicationLayerTests;

public abstract class UpdateAddressHandlerTestBase
{
    protected readonly AddressRepositoryFake Repository = [];

    protected ICommandHandler<UpdateAddressCommand> UpdateAddressService()
    {
        return new UpdateAddressHandler(Repository);
    }
}
