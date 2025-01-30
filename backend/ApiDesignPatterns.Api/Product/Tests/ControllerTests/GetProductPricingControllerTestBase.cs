// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.DomainModels.Views;
using backend.Product.ProductPricingControllers;
using backend.Product.Services.ProductPricingServices;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductPricingControllerTestBase
{
    protected readonly GetProductPricingExtensions Extensions = new();
    protected readonly Fixture Fixture = new();

    protected readonly IQueryHandler<GetProductPricingQuery, ProductPricingView> MockGetProductPricing =
        Mock.Of<IQueryHandler<GetProductPricingQuery, ProductPricingView>>();

    private readonly FieldMaskConverterFactory _fieldMaskConverterFactory =
        new(new ProductPricingFieldPaths().ValidPaths);

    protected GetProductPricingController ProductPricingController()
    {
        return new GetProductPricingController(
            MockGetProductPricing,
            Extensions,
            _fieldMaskConverterFactory);
    }
}
