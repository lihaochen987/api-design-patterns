// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.SupplierControllers;

namespace backend.Supplier.Services;

public class SupplierValueObjectUpdater
{
    public Address GetUpdatedSupplierAddressValues(
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

    public PhoneNumber GetUpdateSupplierPhoneNumberValues(UpdateSupplierRequest request,
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
