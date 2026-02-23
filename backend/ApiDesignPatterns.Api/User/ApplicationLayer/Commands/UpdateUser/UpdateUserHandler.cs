// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.InfrastructureLayer.Database.User;

namespace backend.User.ApplicationLayer.Commands.UpdateUser;

public class UpdateUserHandler(IUserRepository repository) : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand command)
    {
        var request = command.Request;
        var user = command.User;

        var newUser = new DomainModels.User
        {
            Id = user.Id,
            FirstName = request.FieldMask.Contains("firstname") && !string.IsNullOrEmpty(request.FirstName)
                ? request.FirstName
                : user.FirstName,
            LastName = request.FieldMask.Contains("lastname") && !string.IsNullOrEmpty(request.LastName)
                ? request.LastName
                : user.LastName,
            Email = request.FieldMask.Contains("email") && !string.IsNullOrEmpty(request.Email)
                ? request.Email
                : user.Email,
            AddressIds = request.FieldMask.Contains("addressids") && request.AddressIds is { Count: > 0 }
                ? request.AddressIds.ToList()
                : user.AddressIds.ToList(),
            PhoneNumberIds = request.FieldMask.Contains("phonenumberids") && request.PhoneNumberIds is { Count: > 0 }
                ? request.PhoneNumberIds.ToList()
                : user.PhoneNumberIds.ToList(),
            CreatedAt = user.CreatedAt
        };

        await repository.UpdateUserAsync(newUser, user);
    }
}
