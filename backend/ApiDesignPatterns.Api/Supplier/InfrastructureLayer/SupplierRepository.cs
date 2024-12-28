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
    //
    // public async Task CreateSupplierAsync(DomainModels.Supplier supplier)
    // {
    //     context.Suppliers.Add(supplier);
    //     await context.SaveChangesAsync();
    // }
    //
    // public Task UpdateSupplierAsync(DomainModels.Supplier supplier)
    // {
    //     context.Suppliers.Update(supplier);
    //     return context.SaveChangesAsync();
    // }
}
