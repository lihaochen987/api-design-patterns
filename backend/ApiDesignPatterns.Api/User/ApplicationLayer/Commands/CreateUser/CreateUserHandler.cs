// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.InfrastructureLayer.Database.User;

namespace backend.User.ApplicationLayer.Commands.CreateUser;

public class CreateUserHandler(IUserRepository repository) : ICommandHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand command)
    {
        var user = command.User with { CreatedAt = DateTimeOffset.UtcNow };
        await repository.CreateUserAsync(user);
    }
}
