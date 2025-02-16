// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ListProductHandlerTestBase
{
    protected readonly ProductViewRepositoryFake Repository = new(new QueryService<ProductView>());

    protected IQueryHandler<ListProductsQuery, PagedProducts> ListProductsViewHandler()
    {
        return new ListProductsHandler(Repository);
    }
}
