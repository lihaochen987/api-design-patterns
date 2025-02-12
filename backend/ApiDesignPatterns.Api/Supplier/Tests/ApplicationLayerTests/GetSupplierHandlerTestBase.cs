// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.ApplicationLayer.Queries.GetSupplier;
using backend.Supplier.Tests.TestHelpers.Fakes;

namespace backend.Supplier.Tests.ApplicationLayerTests;

public abstract class GetSupplierHandlerTestBase
{
    protected readonly SupplierRepositoryFake Repository = [];

    protected IQueryHandler<GetSupplierQuery, DomainModels.Supplier> GetSupplierHandler()
    {
        return new GetSupplierHandler(Repository);
    }
}
