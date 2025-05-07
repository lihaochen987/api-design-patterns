// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.Controllers.ProductPricing;
using backend.Product.DomainModels.Views;
using backend.Product.Services;
using backend.Product.Services.Mappers;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class GetProductPricingControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected readonly IAsyncQueryHandler<GetProductPricingQuery, ProductPricingView?> MockGetProductPricing =
        Mock.Of<IAsyncQueryHandler<GetProductPricingQuery, ProductPricingView?>>();

    private readonly FieldMaskConverterFactory _fieldMaskConverterFactory =
        new(new ProductPricingFieldPaths().ValidPaths);

    protected GetProductPricingControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductPricingMappings();
        Mapper = new Mapper(config);
    }

    protected GetProductPricingController ProductPricingController()
    {
        return new GetProductPricingController(
            MockGetProductPricing,
            Mapper,
            _fieldMaskConverterFactory);
    }
}
