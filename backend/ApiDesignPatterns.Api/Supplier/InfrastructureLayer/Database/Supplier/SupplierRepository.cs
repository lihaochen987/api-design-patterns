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
        await dbConnection.ExecuteAsync(
            insertAddressQuery,
            new
            {
                SupplierId = supplier.Id,
                supplier.Address.Street,
                supplier.Address.City,
                supplier.Address.PostalCode,
                supplier.Address.Country
            }
        );
    }

    public async Task CreateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    {
        const string insertPhoneNumberQuery = SupplierQueries.CreateSupplierPhoneNumber;
        await dbConnection.ExecuteAsync(
            insertPhoneNumberQuery,
            new
            {
                SupplierId = supplier.Id,
                supplier.PhoneNumber.CountryCode,
                supplier.PhoneNumber.AreaCode,
                supplier.PhoneNumber.Number
            }
        );
    }

    public async Task UpdateSupplierPhoneNumberAsync(DomainModels.Supplier supplier)
    {
        const string updatePhoneNumberQuery = SupplierQueries.UpdateSupplierPhoneNumber;
        await dbConnection.ExecuteAsync(
            updatePhoneNumberQuery,
            new
            {
                SupplierId = supplier.Id,
                supplier.PhoneNumber.CountryCode,
                supplier.PhoneNumber.AreaCode,
                supplier.PhoneNumber.Number
            });
    }

    public async Task UpdateSupplierAddressAsync(DomainModels.Supplier supplier)
    {
        const string updateAddressQuery = SupplierQueries.UpdateSupplierAddress;
        await dbConnection.ExecuteAsync(
            updateAddressQuery,
            new
            {
                SupplierId = supplier.Id,
                supplier.Address.Street,
                supplier.Address.City,
                supplier.Address.PostalCode,
                supplier.Address.Country
            }
        );
    }
}
