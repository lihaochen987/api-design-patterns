// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.UpdateProductPricing;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class UpdateProductPricingHandlerTests : UpdateProductPricingHandlerTestBase
{
    [Fact]
    public async Task Handle_UpdatesAllFields_WhenAllFieldsInMask()
    {
        var product = new ProductTestDataBuilder()
            .WithPricing(new Pricing(100m, 10m, 8m))
            .Build();
        Repository.Add(product);
        var request = new UpdateProductPricingRequest
        {
            BasePrice = "199.99",
            DiscountPercentage = "15.5",
            TaxRate = "9.5",
            FieldMask = ["pricing.baseprice", "pricing.discountpercentage", "pricing.taxrate"]
        };
        var command = new UpdateProductPricingCommand { Product = product, Request = request };
        ICommandHandler<UpdateProductPricingCommand> sut = GetUpdateProductPricingHandler();

        await sut.Handle(command);

        var updatedProduct = await Repository.GetProductAsync(product.Id);
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Pricing.BasePrice.ShouldBe(199.99m);
        updatedProduct.Pricing.DiscountPercentage.ShouldBe(15.5m);
        updatedProduct.Pricing.TaxRate.ShouldBe(9.5m);
    }

    [Fact]
    public async Task Handle_UpdatesOnlyBasePriceAndTaxRate_WhenOnlyThoseFieldsInMask()
    {
        var product = new ProductTestDataBuilder()
            .WithPricing(new Pricing(100m, 10m, 8m))
            .Build();
        Repository.Add(product);
        var request = new UpdateProductPricingRequest
        {
            BasePrice = "199.99",
            DiscountPercentage = "15.5",
            TaxRate = "9.5",
            FieldMask = ["pricing.baseprice", "pricing.taxrate"]
        };
        var command = new UpdateProductPricingCommand { Product = product, Request = request };
        ICommandHandler<UpdateProductPricingCommand> sut = GetUpdateProductPricingHandler();

        await sut.Handle(command);

        var updatedProduct = await Repository.GetProductAsync(product.Id);
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Pricing.BasePrice.ShouldBe(199.99m);
        updatedProduct.Pricing.DiscountPercentage.ShouldBe(10m); // Unchanged
        updatedProduct.Pricing.TaxRate.ShouldBe(9.5m);
    }

    [Fact]
    public async Task Handle_RetainsOriginalValues_WhenFieldsNotInMask()
    {
        var product = new ProductTestDataBuilder()
            .WithPricing(new Pricing(100m, 10m, 8m))
            .Build();
        Repository.Add(product);
        var request = new UpdateProductPricingRequest
        {
            BasePrice = "199.99", DiscountPercentage = "15.5", TaxRate = "9.5", FieldMask = []
        };
        var command = new UpdateProductPricingCommand { Product = product, Request = request };
        ICommandHandler<UpdateProductPricingCommand> sut = GetUpdateProductPricingHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        var updatedProduct = await Repository.GetProductAsync(product.Id);
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Pricing.BasePrice.ShouldBe(100m);
        updatedProduct.Pricing.DiscountPercentage.ShouldBe(10m);
        updatedProduct.Pricing.TaxRate.ShouldBe(8m);
    }

    [Fact]
    public async Task Handle_RetainsOriginalValues_WhenNewValuesNotParseable()
    {
        var product = new ProductTestDataBuilder()
            .WithPricing(new Pricing(100m, 10m, 8m))
            .Build();
        Repository.Add(product);
        var request = new UpdateProductPricingRequest
        {
            BasePrice = "invalid",
            DiscountPercentage = "invalid",
            TaxRate = "invalid",
            FieldMask = ["pricing.baseprice", "pricing.discountpercentage", "pricing.taxrate"]
        };
        var command = new UpdateProductPricingCommand { Product = product, Request = request };
        ICommandHandler<UpdateProductPricingCommand> sut = GetUpdateProductPricingHandler();

        await sut.Handle(command);

        var updatedProduct = await Repository.GetProductAsync(product.Id);
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Pricing.BasePrice.ShouldBe(100m);
        updatedProduct.Pricing.DiscountPercentage.ShouldBe(10m);
        updatedProduct.Pricing.TaxRate.ShouldBe(8m);
    }
}
