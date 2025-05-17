// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.InfrastructureLayer.Database.User;

namespace backend.User.ApplicationLayer.Commands.ReplaceUser;

public class ReplaceUserHandler(IUserRepository repository) : ICommandHandler<ReplaceUserCommand>
{
    public async Task Handle(ReplaceUserCommand command)
    {
        var replacedUser = command.User with { Id = command.UserId, CreatedAt = DateTimeOffset.UtcNow };
        await repository.ReplaceUserAsync(replacedUser);
    }
}
