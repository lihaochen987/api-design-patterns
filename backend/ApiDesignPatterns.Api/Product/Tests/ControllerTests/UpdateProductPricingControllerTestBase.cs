// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.UpdateProductPricing;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.ProductPricing;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class UpdateProductPricingControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected readonly IAsyncQueryHandler<GetProductQuery, DomainModels.Product?> MockGetProductHandler =
        Mock.Of<IAsyncQueryHandler<GetProductQuery, DomainModels.Product?>>();

    private readonly ICommandHandler<UpdateProductPricingCommand> _mockUpdateProductPricingHandler =
        Mock.Of<ICommandHandler<UpdateProductPricingCommand>>();

    protected UpdateProductPricingControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        config.RegisterProductPricingMappings();
        Mapper = new Mapper(config);
    }

    protected UpdateProductPricingController UpdateProductPricingController()
    {
        return new UpdateProductPricingController(
            MockGetProductHandler,
            _mockUpdateProductPricingHandler,
            Mapper);
    }
}
