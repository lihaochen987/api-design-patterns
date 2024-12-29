// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.Services;
using backend.Product.Tests.TestHelpers.Fakes;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ProductApplicationServiceTestBase
{
    protected readonly ProductRepositoryFake Repository = [];
    private readonly ProductFieldMaskConfiguration _configuration = new();
    private readonly UpdateProductService _updateProductService = new();

    protected ProductApplicationService ProductApplicationService()
    {
        return new ProductApplicationService(Repository, _configuration, _updateProductService);
    }
}
