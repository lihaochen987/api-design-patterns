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
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.Supplier>(SupplierQueries.GetSupplier,
            new { Id = id });
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

    public async Task<long> UpdateSupplierAsync(DomainModels.Supplier supplier)
    {
        const string updateSupplierQuery = SupplierQueries.UpdateSupplier;
        return await dbConnection.ExecuteScalarAsync<long>(
            updateSupplierQuery,
            new { supplier.Id, supplier.FirstName, supplier.LastName, supplier.Email }
        );
    }

    public async Task CreateSupplierAddressAsync(DomainModels.Supplier supplier)
    {
        const string insertAddressQuery = SupplierQueries.CreateSupplierAddress;

        var addressParameters = supplier.Addresses.Select(address => new
        {
            SupplierId = supplier.Id,
            address.Street,
            address.City,
            address.PostalCode,
            address.Country
        }).ToList();

        await dbConnection.ExecuteAsync(insertAddressQuery, addressParameters);
    }

    public async Task CreateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    {
        const string insertPhoneNumberQuery = SupplierQueries.CreateSupplierPhoneNumber;

        var phoneParameters = supplier.PhoneNumbers.Select(phone => new
        {
            SupplierId = supplier.Id,
            CountryCode = phone.CountryCode.Value,
            AreaCode = phone.AreaCode.Value,
            Number = phone.Number.Value
        }).ToList();

        await dbConnection.ExecuteAsync(insertPhoneNumberQuery, phoneParameters);
    }

    public async Task UpdateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    {
        const string updatePhoneNumberQuery = SupplierQueries.UpdateSupplierPhoneNumber;
        var phoneParameters = supplier.PhoneNumbers.Select(phone => new
        {
            SupplierId = supplier.Id,
            CountryCode = phone.CountryCode.Value,
            AreaCode = phone.AreaCode.Value,
            Number = phone.Number.Value
        }).ToList();

        await dbConnection.ExecuteAsync(updatePhoneNumberQuery, phoneParameters);
    }

    public async Task UpdateSupplierAddressAsync(DomainModels.Supplier supplier)
    {
        const string updateAddressQuery = SupplierQueries.UpdateSupplierAddress;
        var addressParameters = supplier.Addresses.Select(address => new
        {
            SupplierId = supplier.Id,
            address.Street,
            address.City,
            address.PostalCode,
            address.Country
        }).ToList();

        await dbConnection.ExecuteAsync(updateAddressQuery, addressParameters);
    }
}
