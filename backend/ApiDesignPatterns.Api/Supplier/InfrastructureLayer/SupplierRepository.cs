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
        if (dbConnection.State != ConnectionState.Open)
        {
            dbConnection.Open();
        }

        using var transaction = dbConnection.BeginTransaction();

        try
        {
            const string insertSupplierQuery = SupplierQueries.CreateSupplier;
            supplier.Id = await dbConnection.ExecuteScalarAsync<long>(
                insertSupplierQuery,
                new { supplier.FirstName, supplier.LastName, supplier.Email, supplier.CreatedAt },
                transaction
            );

            if (supplier.Id <= 0)
            {
                throw new Exception("Failed to generate Supplier ID.");
            }

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

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }
    }
    //
    // public Task UpdateSupplierAsync(DomainModels.Supplier supplier)
    // {
    //     context.Suppliers.Update(supplier);
    //     return context.SaveChangesAsync();
    // }
}
