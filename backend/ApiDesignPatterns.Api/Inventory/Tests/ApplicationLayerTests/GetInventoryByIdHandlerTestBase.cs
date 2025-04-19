// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetInventoryByIdHandlerTestBase
{
    protected readonly InventoryRepositoryFake Repository = [];

    protected IQueryHandler<GetInventoryByIdQuery, DomainModels.Inventory?> GetInventoryHandler()
    {
        return new GetInventoryByIdByIdHandler(Repository);
    }
}
