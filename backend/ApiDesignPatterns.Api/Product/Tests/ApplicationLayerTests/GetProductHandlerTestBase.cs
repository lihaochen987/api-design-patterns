// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.GetProduct;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.QueryHandler;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class GetProductHandlerTestBase
{
    protected readonly ProductRepositoryFake Repository = [];

    protected IQueryHandler<GetProductQuery, DomainModels.Product> GetProductHandler()
    {
        return new GetProductHandler(Repository);
    }
}
