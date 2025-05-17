// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.InfrastructureLayer.Database.User;

namespace backend.User.ApplicationLayer.Commands.DeleteUser;

public class DeleteUserHandler(IUserRepository repository) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command)
    {
        await repository.DeleteUserAsync(command.Id);
    }
}
