// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.InfrastructureLayer.Database.Address;
using backend.Shared.CommandHandler;

namespace backend.Address.ApplicationLayer.Commands.DeleteAddress;

public class DeleteAddressHandler(IDeleteAddress repository) : ICommandHandler<DeleteAddressCommand>
{
    public async Task Handle(DeleteAddressCommand command)
    {
        await repository.DeleteAddress(command.Id);
    }
}
