// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Queries;
using Dapper;

namespace backend.Supplier.InfrastructureLayer;

public class SupplierRepository(
    IDbConnection dbConnection)
    : ISupplierRepository
{
    public async Task<DomainModels.Supplier?> GetSupplierAsync(long id)
    {
        const string query = SupplierQueries.GetSupplier;

        var results = await dbConnection.QueryAsync<DomainModels.Supplier, Address, PhoneNumber, DomainModels.Supplier>(
            query,
            (supplier, address, phoneNumber) =>
            {
                supplier.Address = address;
                supplier.PhoneNumber = phoneNumber;
                return supplier;
            },
            new { Id = id },
            splitOn: "Address_SupplierId,PhoneNumber_SupplierId"
        );

        return results.FirstOrDefault();
    }

    public async Task DeleteSupplierAsync(long id)
    {
        await dbConnection.ExecuteAsync(SupplierQueries.DeleteSupplier, new { Id = id });
    }

    public async Task CreateSupplierAsync(DomainModels.Supplier supplier)
    {
        EnsureConnectionIsOpen();

        using var transaction = dbConnection.BeginTransaction();
        try
        {
            supplier.Id = await InsertSupplierAsync(supplier, transaction);
            await InsertSupplierAddressAsync(supplier, transaction);
            await InsertSupplierPhoneNumberAsync(supplier, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            CloseConnectionIfNeeded();
        }
    }

    private void EnsureConnectionIsOpen()
    {
        if (dbConnection.State != ConnectionState.Open)
        {
            dbConnection.Open();
        }
    }

    private void CloseConnectionIfNeeded()
    {
        if (dbConnection.State != ConnectionState.Closed)
        {
            dbConnection.Close();
        }
    }

    private async Task<long> InsertSupplierAsync(DomainModels.Supplier supplier, IDbTransaction transaction)
    {
        const string insertSupplierQuery = SupplierQueries.CreateSupplier;
        return await dbConnection.ExecuteScalarAsync<long>(
            insertSupplierQuery,
            new { supplier.FirstName, supplier.LastName, supplier.Email, supplier.CreatedAt },
            transaction
        );
    }

    private async Task InsertSupplierAddressAsync(DomainModels.Supplier supplier, IDbTransaction transaction)
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

    private async Task InsertSupplierPhoneNumberAsync(DomainModels.Supplier supplier, IDbTransaction transaction)
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
    //
    // public Task UpdateSupplierAsync(DomainModels.Supplier supplier)
    // {
    //     context.Suppliers.Update(supplier);
    //     return context.SaveChangesAsync();
    // }
}
