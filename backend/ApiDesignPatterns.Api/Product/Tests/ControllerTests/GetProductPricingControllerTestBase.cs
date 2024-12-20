// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Product.Tests.TestHelpers.Fakes;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductPricingControllerTestBase
{
    protected readonly GetProductPricingExtensions Extensions = new();
    protected readonly Fixture Fixture = new();
    protected readonly ProductPricingRepositoryFake ProductRepository = [];
    private readonly ProductPricingFieldMaskConfiguration _configuration = new();

    protected GetProductPricingController ProductPricingController()
    {
        return new GetProductPricingController(
            ProductRepository,
            _configuration,
            Extensions);
    }
}
