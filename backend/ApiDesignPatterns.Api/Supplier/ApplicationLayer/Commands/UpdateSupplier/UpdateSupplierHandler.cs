// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.Supplier;
using backend.Supplier.SupplierControllers;

namespace backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;

public class UpdateSupplierHandler(ISupplierRepository repository) : ICommandHandler<UpdateSupplierCommand>
{
    public async Task Handle(UpdateSupplierCommand command)
    {
        (string firstName, string lastName, string email, Address address, PhoneNumber phoneNumber) =
            GetUpdatedSupplierValues(command.Request, command.Supplier);
        var supplier = new DomainModels.Supplier
        {
            Id = command.Supplier.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Address = address,
            CreatedAt = command.Supplier.CreatedAt,
            PhoneNumber = phoneNumber
        };
        await repository.UpdateSupplierAsync(supplier);
        await repository.UpdateSupplierAddressAsync(supplier);
        await repository.UpdateSupplierPhoneNumberAsync(supplier);
    }

    private static (
        string firstName,
        string lastName,
        string email,
        Address address,
        PhoneNumber phoneNumber)
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

        Address address = GetUpdatedSupplierAddressValues(request, supplier);
        PhoneNumber phoneNumber = GetUpdateSupplierPhoneNumberValues(request, supplier);

        return (firstname, lastName, email, address, phoneNumber);
    }

    private static Address GetUpdatedSupplierAddressValues(
        UpdateSupplierRequest request, DomainModels.Supplier supplier)
    {
        string street = request.FieldMask.Contains("street") && !string.IsNullOrEmpty(request.Address?.Street)
            ? request.Address.Street
            : supplier.Address.Street;

        string city = request.FieldMask.Contains("city") && !string.IsNullOrEmpty(request.Address?.City)
            ? request.Address.City
            : supplier.Address.City;

        string postalCode =
            request.FieldMask.Contains("postalcode") && !string.IsNullOrEmpty(request.Address?.PostalCode)
                ? request.Address.PostalCode
                : supplier.Address.PostalCode;

        string country = request.FieldMask.Contains("country") && !string.IsNullOrEmpty(request.Address?.Country)
            ? request.Address.Country
            : supplier.Address.Country;

        return new Address { Street = street, City = city, PostalCode = postalCode, Country = country };
    }

    private static PhoneNumber GetUpdateSupplierPhoneNumberValues(UpdateSupplierRequest request,
        DomainModels.Supplier supplier)
    {
        string countryCode = request.FieldMask.Contains("countrycode") &&
                             !string.IsNullOrEmpty(request.PhoneNumber?.CountryCode)
            ? request.PhoneNumber.CountryCode
            : supplier.PhoneNumber.CountryCode;

        string areaCode = request.FieldMask.Contains("areacode") &&
                          !string.IsNullOrEmpty(request.PhoneNumber?.AreaCode)
            ? request.PhoneNumber.AreaCode
            : supplier.PhoneNumber.AreaCode;

        long number = request.FieldMask.Contains("number") && !string.IsNullOrEmpty(request.PhoneNumber?.Number)
            ? long.Parse(request.PhoneNumber.Number)
            : supplier.PhoneNumber.Number;

        return new PhoneNumber { CountryCode = countryCode, AreaCode = areaCode, Number = number };
    }
}
