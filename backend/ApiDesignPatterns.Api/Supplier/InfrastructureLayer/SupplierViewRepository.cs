// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Queries;
using Dapper;

namespace backend.Supplier.InfrastructureLayer;

public class SupplierViewRepository(IDbConnection dbConnection) : ISupplierViewRepository
{
    public async Task<SupplierView?> GetSupplierView(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<SupplierView>(SupplierViewQueries.GetSupplierView,
            new { Id = id });
    }
}
