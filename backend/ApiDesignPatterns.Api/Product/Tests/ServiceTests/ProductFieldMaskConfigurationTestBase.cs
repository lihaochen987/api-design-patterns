// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Services.ProductServices;
using backend.Product.Tests.TestHelpers.Builders;

namespace backend.Product.Tests.ServiceTests;

public class ProductFieldMaskConfigurationTestBase
{
    private readonly IProductPricingFieldMaskService _productPricingFieldMaskService;
    private readonly DimensionsFieldMaskService _dimensionsFieldMaskService;

    protected ProductFieldMaskConfigurationTestBase()
    {
        _productPricingFieldMaskService = new ProductPricingFieldMaskService();
        _dimensionsFieldMaskService = new DimensionsFieldMaskService();
        IFixture fixture = new Fixture();
        fixture.Customizations.Add(new ProductDimensionsBuilder());
    }

    protected ProductFieldMaskConfiguration ProductFieldMaskConfiguration()
    {
        return new ProductFieldMaskConfiguration(_productPricingFieldMaskService, _dimensionsFieldMaskService);
    }
}
