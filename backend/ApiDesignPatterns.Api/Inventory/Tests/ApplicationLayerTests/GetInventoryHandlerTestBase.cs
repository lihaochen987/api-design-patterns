// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetInventory;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetInventoryHandlerTestBase
{
    protected readonly InventoryRepositoryFake Repository = [];

    protected IQueryHandler<GetInventoryQuery, DomainModels.Inventory?> GetInventoryHandler()
    {
        return new GetInventoryHandler(Repository);
    }
}
