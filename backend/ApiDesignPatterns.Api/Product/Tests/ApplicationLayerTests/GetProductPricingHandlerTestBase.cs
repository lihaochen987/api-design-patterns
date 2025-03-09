// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class GetProductPricingHandlerTestBase
{
    protected readonly ProductPricingRepositoryFake Repository = [];

    protected IQueryHandler<GetProductPricingQuery, ProductPricingView?> GetProductPricingHandler()
    {
        return new GetProductPricingHandler(Repository);
    }
}
