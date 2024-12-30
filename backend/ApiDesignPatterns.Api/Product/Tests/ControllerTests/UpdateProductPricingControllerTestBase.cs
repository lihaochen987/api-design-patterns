// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ProductPricingControllers;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Tests.TestHelpers.Fakes;

namespace backend.Product.Tests.ControllerTests;

public abstract class UpdateProductPricingControllerTestBase
{
    protected readonly UpdateProductPricingExtensions Extensions = new();
    protected readonly Fixture Fixture = new();
    protected readonly ProductRepositoryFake ProductRepository = [];
    private readonly ProductPricingFieldMaskConfiguration _configuration = new();

    protected UpdateProductPricingController UpdateProductPricingController()
    {
        return new UpdateProductPricingController(
            ProductRepository,
            _configuration,
            Extensions);
    }
}
