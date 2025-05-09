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
        (FirstName firstName, LastName lastName, Email email, List<long> addressIds, List<long> phoneNumberIds) =
            GetUpdatedSupplierValues(command.Request, command.Supplier);
        var newSupplier = new DomainModels.Supplier
        {
            Id = command.Supplier.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            AddressIds = addressIds,
            CreatedAt = command.Supplier.CreatedAt,
            PhoneNumberIds = phoneNumberIds
        };
        await repository.UpdateSupplierAsync(newSupplier, command.Supplier);
    }

    private static (FirstName firstName, LastName lastName, Email email, List<long> addressIds, List<long> phoneNumberIds)
        GetUpdatedSupplierValues(
            UpdateSupplierRequest request,
            DomainModels.Supplier supplier)
    {
        FirstName firstname = request.FieldMask.Contains("firstname") && !string.IsNullOrEmpty(request.FirstName)
            ? new FirstName(request.FirstName)
            : supplier.FirstName;

        LastName lastName = request.FieldMask.Contains("lastname") && !string.IsNullOrEmpty(request.LastName)
            ? new LastName(request.LastName)
            : supplier.LastName;

        Email email = request.FieldMask.Contains("email") && !string.IsNullOrEmpty(request.Email)
            ? new Email(request.Email)
            : supplier.Email;

        var addresses = GetUpdatedSupplierAddresses(request, supplier);
        var phoneNumbers = GetUpdatedSupplierPhoneNumbers(request, supplier);

        return (firstname, lastName, email, addresses, phoneNumbers);
    }

    private static List<long> GetUpdatedSupplierAddresses(
        UpdateSupplierRequest request,
        DomainModels.Supplier supplier)
    {
        if (request.FieldMask.Contains("addressids") && request.AddressIds != null && request.AddressIds.Count != 0)
        {
            return request.AddressIds.Select(p => p).ToList();
        }

        return supplier.AddressIds.ToList();
    }

    private static List<long> GetUpdatedSupplierPhoneNumbers(
        UpdateSupplierRequest request,
        DomainModels.Supplier supplier)
    {
        if (request.FieldMask.Contains("phonenumberids") && request.PhoneNumberIds != null &&
            request.PhoneNumberIds.Count != 0)
        {
            return request.PhoneNumberIds.Select(p => p).ToList();
        }

        return supplier.PhoneNumberIds.ToList();
    }
}
