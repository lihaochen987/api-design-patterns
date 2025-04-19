// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetInventoryByProductAndSupplierHandlerTestBase
{
    protected readonly InventoryRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?> GetHandler()
    {
        return new GetInventoryByProductAndSupplierHandler(Repository);
    }
}
