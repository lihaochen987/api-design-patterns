// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;
using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Fakes;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public abstract class GetSupplierViewHandlerTestBase
{
    protected readonly SupplierViewRepositoryFake Repository = new(new PaginateService<SupplierView>());

    protected IQueryHandler<GetSupplierViewQuery, SupplierView?> GetSupplierViewHandler()
    {
        return new GetSupplierViewHandler(Repository);
    }
}
