// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ProductPricingControllers;
using backend.Product.Services.ProductPricingServices;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.FieldMask;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductPricingControllerTestBase
{
    protected readonly GetProductPricingExtensions Extensions = new();
    protected readonly Fixture Fixture = new();
    protected readonly ProductPricingRepositoryFake ProductRepository = [];
    private readonly ProductPricingFieldPaths _fieldPaths = new();

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new FieldMaskExpander(), new PropertyHandler(new NestedJObjectBuilder()));

    protected GetProductPricingController ProductPricingController()
    {
        return new GetProductPricingController(
            ProductRepository,
            _fieldPaths,
            Extensions,
            _fieldMaskConverterFactory);
    }
}
