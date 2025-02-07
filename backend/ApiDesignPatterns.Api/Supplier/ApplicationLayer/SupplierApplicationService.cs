// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer;
using backend.Supplier.InfrastructureLayer.Database.Supplier;
using backend.Supplier.Services;
using backend.Supplier.SupplierControllers;

namespace backend.Supplier.ApplicationLayer;

public class SupplierApplicationService(
    ISupplierRepository repository,
    SupplierFieldMaskConfiguration maskConfiguration)
    : ISupplierApplicationService
{
    public async Task ReplaceSupplierAsync(DomainModels.Supplier supplier, long id)
    {
        supplier.Id = id;
        await repository.UpdateSupplierAsync(supplier);
    }

    public async Task UpdateSupplierAsync(UpdateSupplierRequest request, DomainModels.Supplier supplier, long id)
    {
        (string firstName, string lastName, string email, Address address, PhoneNumber phoneNumber) =
            maskConfiguration.GetUpdatedSupplierValues(request, supplier);
        supplier.FirstName = firstName;
        supplier.LastName = lastName;
        supplier.Email = email;
        supplier.Address = address;
        supplier.PhoneNumber = phoneNumber;
        supplier.Id = id;
        await repository.UpdateSupplierAsync(supplier);
    }
}
