// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.DomainModels;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class ListInventoryHandlerTestBase
{
    protected readonly InventoryViewRepositoryFake Repository = new(new PaginateService<InventoryView>());

    protected readonly Fixture Fixture = new();

    protected IAsyncQueryHandler<ListInventoryQuery, PagedInventory> ListInventoryViewHandler()
    {
        return new ListInventoryHandler(Repository);
    }
}
