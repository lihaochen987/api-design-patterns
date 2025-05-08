// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Supplier.InfrastructureLayer.Database.Supplier;

public class SupplierRepository(
    IDbConnection dbConnection)
    : ISupplierRepository
{
    public async Task<DomainModels.Supplier?> GetSupplierAsync(long id)
    {
        var supplier = await dbConnection.QuerySingleOrDefaultAsync<DomainModels.Supplier>(SupplierQueries.GetSupplier,
            new { Id = id });
        if (supplier == null)
        {
            return null;
        }

        var phoneNumberIds = await GetSupplierPhoneNumberIds(id);
        var addressIds = await GetSupplierAddressIds(id);
        var hydratedSupplier = supplier with { PhoneNumberIds = phoneNumberIds, AddressIds = addressIds };
        return hydratedSupplier;
    }

    public async Task DeleteSupplierAsync(long id)
    {
        await dbConnection.ExecuteAsync(SupplierQueries.DeleteSupplier, new { Id = id });
    }

    public async Task<long> CreateSupplierAsync(DomainModels.Supplier supplier)
    {
        const string insertSupplierQuery = SupplierQueries.CreateSupplier;
        return await dbConnection.ExecuteScalarAsync<long>(
            insertSupplierQuery,
            new { supplier.FirstName, supplier.LastName, supplier.Email, supplier.CreatedAt }
        );
    }

    public async Task<long> UpdateSupplierAsync(DomainModels.Supplier newSupplier, DomainModels.Supplier oldSupplier)
    {
        const string updateSupplierQuery = SupplierQueries.UpdateSupplier;
        long supplierId = await dbConnection.ExecuteScalarAsync<long>(
            updateSupplierQuery,
            new { newSupplier.Id, newSupplier.FirstName, newSupplier.LastName, newSupplier.Email }
        );
        await UpdateSupplierPhoneNumberIds(newSupplier.PhoneNumberIds, oldSupplier.PhoneNumberIds, supplierId);
        await UpdateSupplierAddressIds(newSupplier.AddressIds, oldSupplier.AddressIds, supplierId);

        return supplierId;
    }

    public async Task<long> ReplaceSupplierAsync(DomainModels.Supplier supplier)
    {
        const string updateSupplierQuery = SupplierQueries.UpdateSupplier;
        long supplierId = await dbConnection.ExecuteScalarAsync<long>(
            updateSupplierQuery,
            new { supplier.Id, supplier.FirstName, supplier.LastName, supplier.Email }
        );
        return supplierId;
    }

    private async Task UpdateSupplierPhoneNumberIds(
        ICollection<long> newPhoneNumberIds,
        ICollection<long> oldPhoneNumberIds,
        long supplierId)
    {
        await dbConnection.ExecuteAsync(
            SupplierQueries.UpdateOldSupplierPhoneNumberId,
            new { PhoneNumberIds = oldPhoneNumberIds }
        );

        const string updatePhoneNumberQuery = SupplierQueries.UpdateSupplierPhoneNumberId;
        var phoneParameters = newPhoneNumberIds.Select(phoneNumberId => new
        {
            PhoneNumberId = phoneNumberId, SupplierId = supplierId,
        }).ToList();

        await dbConnection.ExecuteAsync(updatePhoneNumberQuery, phoneParameters);
    }

    private async Task UpdateSupplierAddressIds(
        ICollection<long> newAddressIds,
        ICollection<long> oldAddressIds,
        long supplierId)
    {
        await dbConnection.ExecuteAsync(
            SupplierQueries.UpdateOldSupplierAddressId,
            new { AddressIds = oldAddressIds }
        );

        const string updateAddressQuery = SupplierQueries.UpdateSupplierAddressId;
        var addressParameters = newAddressIds.Select(addressId => new
        {
            AddressId = addressId, SupplierId = supplierId,
        }).ToList();

        await dbConnection.ExecuteAsync(updateAddressQuery, addressParameters);
    }

    private async Task<List<long>> GetSupplierPhoneNumberIds(long supplierId)
    {
        var phoneNumberIds = await dbConnection.QueryAsync<long>(
            SupplierQueries.GetSupplierPhoneNumberIds,
            new { SupplierId = supplierId });
        return phoneNumberIds.ToList();
    }

    private async Task<List<long>> GetSupplierAddressIds(long supplierId)
    {
        var addressIds = await dbConnection.QueryAsync<long>(
            SupplierQueries.GetSupplierAddressIds,
            new { SupplierId = supplierId });
        return addressIds.ToList();
    }

    // public async Task CreateSupplierAddressAsync(DomainModels.Supplier supplier)
    // {
    //     const string insertAddressQuery = SupplierQueries.CreateSupplierAddress;
    //
    //     var addressParameters = supplier.Addresses.Select(address => new
    //     {
    //         SupplierId = supplier.Id,
    //         address.Street,
    //         address.City,
    //         address.PostalCode,
    //         address.Country
    //     }).ToList();
    //
    //     await dbConnection.ExecuteAsync(insertAddressQuery, addressParameters);
    // }

    // public async Task CreateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    // {
    //     const string insertPhoneNumberQuery = SupplierQueries.CreateSupplierPhoneNumber;
    //
    //     var phoneParameters = supplier.PhoneNumbers.Select(phone => new
    //     {
    //         SupplierId = supplier.Id,
    //         CountryCode = phone.CountryCode.Value,
    //         AreaCode = phone.AreaCode.Value,
    //         Number = phone.Number.Value
    //     }).ToList();
    //
    //     await dbConnection.ExecuteAsync(insertPhoneNumberQuery, phoneParameters);
    // }

    // public async Task UpdateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    // {
    //     const string updatePhoneNumberQuery = SupplierQueries.UpdateSupplierPhoneNumber;
    //     var phoneParameters = supplier.PhoneNumbers.Select(phone => new
    //     {
    //         SupplierId = supplier.Id,
    //         CountryCode = phone.CountryCode.Value,
    //         AreaCode = phone.AreaCode.Value,
    //         Number = phone.Number.Value
    //     }).ToList();
    //
    //     await dbConnection.ExecuteAsync(updatePhoneNumberQuery, phoneParameters);
    // }

    // public async Task UpdateSupplierAddressAsync(DomainModels.Supplier supplier)
    // {
    //     const string updateAddressQuery = SupplierQueries.UpdateSupplierAddress;
    //     var addressParameters = supplier.Addresses.Select(address => new
    //     {
    //         SupplierId = supplier.Id,
    //         address.Street,
    //         address.City,
    //         address.PostalCode,
    //         address.Country
    //     }).ToList();
    //
    //     await dbConnection.ExecuteAsync(updateAddressQuery, addressParameters);
    // }
}
