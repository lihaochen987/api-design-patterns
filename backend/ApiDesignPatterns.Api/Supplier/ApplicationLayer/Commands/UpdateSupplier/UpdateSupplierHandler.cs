// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.Controllers;
using backend.Supplier.DomainModels.ValueObjects;
using backend.Supplier.InfrastructureLayer.Database.Supplier;

namespace backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;

public class UpdateSupplierHandler(ISupplierRepository repository) : ICommandHandler<UpdateSupplierCommand>
{
    public async Task Handle(UpdateSupplierCommand command)
    {
        (string firstName, string lastName, string email, List<Address> addresses, List<PhoneNumber> phoneNumbers) =
            GetUpdatedSupplierValues(command.Request, command.Supplier);
        var supplier = new DomainModels.Supplier
        {
            Id = command.Supplier.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Addresses = addresses,
            CreatedAt = command.Supplier.CreatedAt,
            PhoneNumbers = phoneNumbers
        };
        await repository.UpdateSupplierAsync(supplier);
        await repository.UpdateSupplierAddressAsync(supplier);
        await repository.UpdateSupplierPhoneNumberAsync(supplier);
    }

    private static (
        string firstName,
        string lastName,
        string email,
        List<Address> addresses,
        List<PhoneNumber> phoneNumbers)
        GetUpdatedSupplierValues(
            UpdateSupplierRequest request,
            DomainModels.Supplier supplier)
    {
        string firstname = request.FieldMask.Contains("firstname") && !string.IsNullOrEmpty(request.FirstName)
            ? request.FirstName
            : supplier.FirstName;

        string lastName = request.FieldMask.Contains("lastname") && !string.IsNullOrEmpty(request.LastName)
            ? request.LastName
            : supplier.LastName;

        string email = request.FieldMask.Contains("email") && !string.IsNullOrEmpty(request.Email)
            ? request.Email
            : supplier.Email;

        List<Address> addresses = GetUpdatedSupplierAddresses(request, supplier);

        List<PhoneNumber> phoneNumbers = GetUpdatedSupplierPhoneNumbers(request, supplier);

        return (firstname, lastName, email, addresses, phoneNumbers);
    }

    private static List<Address> GetUpdatedSupplierAddresses(
        UpdateSupplierRequest request,
        DomainModels.Supplier supplier)
    {
        if (request.FieldMask.Contains("addresses") && request.Addresses != null && request.Addresses.Count != 0)
        {
            return request.Addresses.Select(a => new Address
            {
                Street = !string.IsNullOrEmpty(a.Street) ? new Street(a.Street) : new Street(""),
                City = !string.IsNullOrEmpty(a.City) ? new City(a.City) : new City(""),
                PostalCode =
                    !string.IsNullOrEmpty(a.PostalCode) ? new PostalCode(a.PostalCode) : new PostalCode(""),
                Country = !string.IsNullOrEmpty(a.Country) ? new Country(a.Country) : new Country("")
            }).ToList();
        }

        return supplier.Addresses.ToList();
    }

    private static List<PhoneNumber> GetUpdatedSupplierPhoneNumbers(
        UpdateSupplierRequest request,
        DomainModels.Supplier supplier)
    {
        if (request.FieldMask.Contains("phonenumbers") && request.PhoneNumbers != null && request.PhoneNumbers.Any())
        {
            return request.PhoneNumbers.Select(p => new PhoneNumber
            {
                CountryCode =
                    !string.IsNullOrEmpty(p.CountryCode) ? new CountryCode(p.CountryCode) : new CountryCode(""),
                AreaCode = !string.IsNullOrEmpty(p.AreaCode) ? new AreaCode(p.AreaCode) : new AreaCode(""),
                Number = p.Number.HasValue ? new PhoneDigits(p.Number.Value) : new PhoneDigits(0)
            }).ToList();
        }

        return supplier.PhoneNumbers.ToList();
    }
}
