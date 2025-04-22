// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;
using backend.Shared;
using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Fakes;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetSuppliersByIdsHandlerTestBase
{
    protected readonly SupplierViewRepositoryFake Repository = new(new PaginateService<SupplierView>());
    protected readonly Fixture Fixture = new();

    protected IAsyncQueryHandler<GetSuppliersByIdsQuery, List<SupplierView>> GetSuppliersByIdsHandler()
    {
        return new GetSuppliersByIdsHandler(Repository);
    }
}
