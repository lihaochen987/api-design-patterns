// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer.Commands.UpdateProductPricing;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.ProductPricing;
using backend.Product.Services.Mappers;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
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
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ProductPricingMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected UpdateProductPricingController UpdateProductPricingController()
    {
        return new UpdateProductPricingController(
            MockGetProductHandler,
            _mockUpdateProductPricingHandler,
            Mapper);
    }
}
