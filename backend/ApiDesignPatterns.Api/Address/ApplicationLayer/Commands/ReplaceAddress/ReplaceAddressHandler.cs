// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.InfrastructureLayer.Database.Address;
using backend.Shared.CommandHandler;

namespace backend.Address.ApplicationLayer.Commands.ReplaceAddress;

public class ReplaceAddressHandler(IAddressRepository repository) : ICommandHandler<ReplaceAddressCommand>
{
    public async Task Handle(ReplaceAddressCommand command)
    {
        await repository.UpdateAddressAsync(command.Address);
    }
}
