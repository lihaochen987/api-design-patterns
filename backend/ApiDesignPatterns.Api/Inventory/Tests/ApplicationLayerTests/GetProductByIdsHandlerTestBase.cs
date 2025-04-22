// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetProductsByIds;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class GetProductsByIdsHandlerTestBase
{
    protected Fixture Fixture = new();
    protected readonly ProductViewRepositoryFake Repository = new(new PaginateService<ProductView>());

    protected IAsyncQueryHandler<GetProductsByIdsQuery, List<ProductView>> GetProductsByIdsHandler()
    {
        return new GetProductsByIdsHandler(Repository);
    }
}
