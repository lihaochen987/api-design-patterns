// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.DomainModels;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetInventoryViewHandlerTestBase
{
    protected readonly InventoryViewRepositoryFake Repository = new(new PaginateService<InventoryView>());

    protected IAsyncQueryHandler<GetInventoryViewQuery, InventoryView?> GetInventoryViewHandler()
    {
        return new GetInventoryViewHandler(Repository);
    }
}
