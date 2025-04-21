// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetSuppliersFromInventory;
using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetSuppliersFromInventoryHandlerTestBase
{
    protected ISyncQueryHandler<GetSuppliersFromInventoryQuery, List<SupplierView?>> GetSuppliersFromInventoryHandler()
    {
        return new GetSuppliersFromInventoryHandler();
    }
}
