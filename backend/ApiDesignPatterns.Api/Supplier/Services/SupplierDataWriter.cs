// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using backend.Supplier.InfrastructureLayer.Queries;
using Dapper;

namespace backend.Supplier.Services;

public class SupplierDataWriter(IDbConnection dbConnection)
{
    public async Task<long> CreateSupplier(DomainModels.Supplier supplier, IDbTransaction transaction)
    {
        const string insertSupplierQuery = SupplierQueries.CreateSupplier;
        return await dbConnection.ExecuteScalarAsync<long>(
            insertSupplierQuery,
            new { supplier.FirstName, supplier.LastName, supplier.Email, supplier.CreatedAt },
            transaction
        );
    }

    public async Task CreateSupplierAddress(DomainModels.Supplier supplier, IDbTransaction transaction)
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
            },
            transaction
        );
    }

    public async Task CreateSupplierPhoneNumber(DomainModels.Supplier supplier, IDbTransaction transaction)
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
            },
            transaction
        );
    }

    public async Task UpdateSupplierPhoneNumber(DomainModels.Supplier supplier, IDbTransaction transaction)
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
            },
            transaction
        );
    }

    public async Task UpdateSupplierAddress(DomainModels.Supplier supplier, IDbTransaction transaction)
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
            },
            transaction
        );
    }

    public async Task<long> UpdateSupplier(DomainModels.Supplier supplier, IDbTransaction transaction)
    {
        const string updateSupplierQuery = SupplierQueries.UpdateSupplier;
        return await dbConnection.ExecuteScalarAsync<long>(
            updateSupplierQuery,
            new { supplier.Id, supplier.FirstName, supplier.LastName, supplier.Email },
            transaction
        );
    }
}
