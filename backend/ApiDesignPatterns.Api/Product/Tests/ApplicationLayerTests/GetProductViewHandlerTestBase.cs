// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductView;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class GetProductViewHandlerTestBase
{
    protected readonly ProductViewRepositoryFake Repository = new(new QueryService<ProductView>());
    protected readonly IFixture Fixture = new Fixture();

    protected IQueryHandler<GetProductViewQuery, ProductView> GetProductViewHandler()
    {
        return new GetProductViewHandler(Repository);
    }
}
