// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.Services;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Services.ProductServices;
using backend.Product.Tests.TestHelpers.Fakes;

namespace backend.Product.Tests.ApplicationLayerTests;

public abstract class ProductApplicationServiceTestBase
{
    protected readonly ProductRepositoryFake Repository = [];

    private readonly UpdateProductTypeService _updateProductTypeService =
        new(new ProductFieldMaskConfiguration(new ProductPricingFieldMaskService(), new DimensionsFieldMaskService()));

    protected ProductApplicationService ProductApplicationService()
    {
        return new ProductApplicationService(Repository, _updateProductTypeService);
    }
}
