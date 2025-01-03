// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using AutoMapper;
using backend.Supplier.InfrastructureLayer.Queries;
using backend.Supplier.Services;
using Dapper;

namespace backend.Supplier.InfrastructureLayer;

public class SupplierRepository(
    IDbConnection dbConnection,
    SupplierDataWriter dataWriter,
    IMapper mapper)
    : ISupplierRepository
{
    public async Task<DomainModels.Supplier?> GetSupplierAsync(long id)
    {
        var result =
            await dbConnection.QueryFirstOrDefaultAsync<DomainModels.Supplier>(SupplierQueries.GetSupplier,
                new { Id = id });

        return result == null ? null : mapper.Map<DomainModels.Supplier>(result);
    }

    public async Task DeleteSupplierAsync(long id)
    {
        await dbConnection.ExecuteAsync(SupplierQueries.DeleteSupplier, new { Id = id });
    }

    public async Task CreateSupplierAsync(DomainModels.Supplier supplier)
    {
        dbConnection.Open();

        using var transaction = dbConnection.BeginTransaction();
        try
        {
            supplier.Id = await dataWriter.CreateSupplier(supplier, transaction);
            await dataWriter.CreateSupplierAddress(supplier, transaction);
            await dataWriter.CreateSupplierPhoneNumber(supplier, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            dbConnection.Close();
        }
    }

    public async Task UpdateSupplierAsync(DomainModels.Supplier supplier)
    {
        dbConnection.Open();

        using var transaction = dbConnection.BeginTransaction();
        try
        {
            supplier.Id = await dataWriter.UpdateSupplier(supplier, transaction);
            await dataWriter.UpdateSupplierAddress(supplier, transaction);
            await dataWriter.UpdateSupplierPhoneNumber(supplier, transaction);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            dbConnection.Close();
        }
    }
}
