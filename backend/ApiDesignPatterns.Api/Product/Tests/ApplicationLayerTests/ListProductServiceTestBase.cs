// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.GetProductView;
using backend.Product.ApplicationLayer.ListProducts;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ListProductServiceTestBase
{
    protected readonly ProductViewRepositoryFake Repository;
    protected readonly IFixture Fixture;

    protected ListProductServiceTestBase()
    {
        Fixture = new Fixture();
        Repository = new ProductViewRepositoryFake(new QueryService<ProductView>());
    }

    protected IQueryHandler<ListProductsQuery, (List<ProductView>, string?)> ListProductsViewHandler()
    {
        return new ListProductsHandler(Repository);
    }
}
