// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.InfrastructureLayer.Database.PhoneNumber;
using backend.Shared.CommandHandler;

namespace backend.PhoneNumber.ApplicationLayer.Commands.DeletePhoneNumber;

public class DeletePhoneNumberHandler(IPhoneNumberRepository repository) : ICommandHandler<DeletePhoneNumberCommand>
{
    public async Task Handle(DeletePhoneNumberCommand command)
    {
        await repository.DeletePhoneNumber(command.Id);
    }
}
