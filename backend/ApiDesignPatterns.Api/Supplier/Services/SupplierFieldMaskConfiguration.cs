// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.SupplierControllers;

namespace backend.Supplier.Services;

public class SupplierFieldMaskConfiguration(SupplierValueObjectUpdater supplierValueObjectUpdater)
{
    public (
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

        Address address = supplierValueObjectUpdater.GetUpdatedSupplierAddressValues(request, supplier);
        PhoneNumber phoneNumber = supplierValueObjectUpdater.GetUpdateSupplierPhoneNumberValues(request, supplier);

        return (firstname, lastName, email, address, phoneNumber);
    }
}
