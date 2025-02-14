// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.ListSuppliers;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Fakes;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public abstract class ListSuppliersHandlerTestBase
{
    protected readonly SupplierViewRepositoryFake Repository = new(new QueryService<SupplierView>());
    protected readonly Fixture Fixture = new();

    protected IQueryHandler<ListSuppliersQuery, (List<SupplierView>, string?)> ListSuppliersViewHandler()
    {
        return new ListSuppliersHandler(Repository);
    }
}
