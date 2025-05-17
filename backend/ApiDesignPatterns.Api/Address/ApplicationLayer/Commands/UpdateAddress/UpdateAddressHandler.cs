// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.Controllers;
using backend.Address.DomainModels.ValueObjects;
using backend.Address.InfrastructureLayer.Database.Address;
using backend.Shared.CommandHandler;

namespace backend.Address.ApplicationLayer.Commands.UpdateAddress;

public class UpdateAddressHandler(IAddressRepository repository) : ICommandHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand command)
    {
        (long userId, Street street, City city, PostalCode postalCode, Country country) =
            GetUpdatedAddressValues(command.Request, command.Address);
        var address = new DomainModels.Address
        {
            Id = command.Address.Id,
            UserId = userId,
            Street = street,
            City = city,
            PostalCode = postalCode,
            Country = country
        };
        await repository.UpdateAddressAsync(address);
    }

    private static (long userId, Street street, City city, PostalCode postalCode, Country country)
        GetUpdatedAddressValues(UpdateAddressRequest request, DomainModels.Address address)
    {
        long userId = request.FieldMask.Contains("userid") && request.UserId != null
            ? request.UserId.Value
            : address.UserId;

        Street street = request.FieldMask.Contains("street") && request.Street != null
            ? new Street(request.Street)
            : address.Street;

        City city = request.FieldMask.Contains("city") && request.City != null
            ? new City(request.City)
            : address.City;

        PostalCode postalCode = request.FieldMask.Contains("postalcode") && request.PostalCode != null
            ? new PostalCode(request.PostalCode)
            : address.PostalCode;

        Country country = request.FieldMask.Contains("country") && request.Country != null
            ? new Country(request.Country)
            : address.Country;
        return (userId, street, city, postalCode, country);
    }
}
