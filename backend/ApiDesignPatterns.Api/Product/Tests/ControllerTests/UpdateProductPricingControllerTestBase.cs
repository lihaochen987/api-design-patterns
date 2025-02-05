// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Commands.UpdateProductPricing;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Product.Tests.ControllerTests;

public abstract class UpdateProductPricingControllerTestBase
{
    protected readonly UpdateProductPricingExtensions Extensions = new();
    protected readonly Fixture Fixture = new();

    protected readonly IQueryHandler<GetProductQuery, DomainModels.Product> MockGetProductHandler =
        Mock.Of<IQueryHandler<GetProductQuery, DomainModels.Product>>();
    protected readonly ICommandHandler<UpdateProductPricingQuery> MockUpdateProductPricingHandler =
        Mock.Of<ICommandHandler<UpdateProductPricingQuery>>();

    protected UpdateProductPricingController UpdateProductPricingController()
    {
        return new UpdateProductPricingController(
            MockGetProductHandler,
            MockUpdateProductPricingHandler,
            Extensions);
    }
}
