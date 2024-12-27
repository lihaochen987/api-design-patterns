// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ProductViewApplicationServiceTestBase
{
    protected readonly ProductViewRepositoryFake Repository;
    protected readonly IFixture Fixture;

    protected ProductViewApplicationServiceTestBase()
    {
        Fixture = new Fixture();
        Repository = new ProductViewRepositoryFake(new QueryService<ProductView>());
    }

    protected ProductViewApplicationService ProductViewApplicationService()
    {
        return new ProductViewApplicationService(Repository);
    }
}
