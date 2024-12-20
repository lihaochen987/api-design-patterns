// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.Tests.TestHelpers.Fakes;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ProductApplicationServiceTestBase
{
    protected readonly ProductRepositoryFake Repository = [];

    protected ProductApplicationService ProductApplicationService()
    {
        return new ProductApplicationService(Repository);
    }
}
