// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.Controllers;
using backend.User.DomainModels.ValueObjects;
using backend.User.InfrastructureLayer.Database.User;

namespace backend.User.ApplicationLayer.Commands.UpdateUser;

public class UpdateUserHandler(IUserRepository repository) : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand command)
    {
        (FirstName firstName, LastName lastName, Email email, List<long> addressIds, List<long> phoneNumberIds) =
            GetUpdatedUserValues(command.Request, command.User);
        var newUser = new DomainModels.User
        {
            Id = command.User.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            AddressIds = addressIds,
            CreatedAt = command.User.CreatedAt,
            PhoneNumberIds = phoneNumberIds
        };
        await repository.UpdateUserAsync(newUser, command.User);
    }

    private static (FirstName firstName, LastName lastName, Email email, List<long> addressIds, List<long> phoneNumberIds)
        GetUpdatedUserValues(
            UpdateUserRequest request,
            DomainModels.User user)
    {
        FirstName firstname = request.FieldMask.Contains("firstname") && !string.IsNullOrEmpty(request.FirstName)
            ? new FirstName(request.FirstName)
            : user.FirstName;

        LastName lastName = request.FieldMask.Contains("lastname") && !string.IsNullOrEmpty(request.LastName)
            ? new LastName(request.LastName)
            : user.LastName;

        Email email = request.FieldMask.Contains("email") && !string.IsNullOrEmpty(request.Email)
            ? new Email(request.Email)
            : user.Email;

        var addresses = GetUpdatedUserAddresses(request, user);
        var phoneNumbers = GetUpdatedUserPhoneNumbers(request, user);

        return (firstname, lastName, email, addresses, phoneNumbers);
    }

    private static List<long> GetUpdatedUserAddresses(
        UpdateUserRequest request,
        DomainModels.User user)
    {
        if (request.FieldMask.Contains("addressids") && request.AddressIds != null && request.AddressIds.Count != 0)
        {
            return request.AddressIds.Select(p => p).ToList();
        }

        return user.AddressIds.ToList();
    }

    private static List<long> GetUpdatedUserPhoneNumbers(
        UpdateUserRequest request,
        DomainModels.User user)
    {
        if (request.FieldMask.Contains("phonenumberids") && request.PhoneNumberIds != null &&
            request.PhoneNumberIds.Count != 0)
        {
            return request.PhoneNumberIds.Select(p => p).ToList();
        }

        return user.PhoneNumberIds.ToList();
    }
}
