// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class UpdateProductHandlerTests : UpdateProductHandlerTestBase
{
    [Fact]
    public async Task UpdateProductAsync_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithId(3).WithName(new Name("Original Name"))
            .WithPricing(new Pricing(20.99m, 5m, 3m))
            .WithCategory(Category.Feeders).Build();
        Repository.Add(product);
        Repository.IsDirty = false;
        UpdateProductRequest request = new()
        {
            Name = "Updated Name",
            Pricing = new ProductPricingRequest { BasePrice = 25.50m, DiscountPercentage = 50, TaxRate = 15 },
            Category = "Toys",
            FieldMask = ["name", "category", "discountpercentage", "taxrate"]
        };
        ICommandHandler<UpdateProductCommand> sut = UpdateProductService();

        await sut.Handle(new UpdateProductCommand { Product = product, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Name.Value.Should().Be(request.Name);
        Repository.First().Category.Should().Be((Category)Enum.Parse(typeof(Category), request.Category));
        Repository.First().Pricing.DiscountPercentage.Should().Be(request.Pricing.DiscountPercentage);
        Repository.First().Pricing.TaxRate.Should().Be(request.Pricing.TaxRate);
    }

    [Fact]
    public async Task UpdateProductAsync_WithNestedFieldInFieldMask_ShouldUpdateNestedField()
    {
        DomainModels.Product product = new ProductTestDataBuilder()
            .WithId(5).WithDimensions(new Dimensions(10, 5, 2)).Build();
        Repository.Add(product);
        Repository.IsDirty = false;
        UpdateProductRequest request = new()
        {
            Dimensions = new DimensionsRequest { Length = 20, Width = 10, Height = 2 },
            FieldMask = ["dimensions.width", "dimensions.height"]
        };
        ICommandHandler<UpdateProductCommand> sut = UpdateProductService();

        await sut.Handle(new UpdateProductCommand { Product = product, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Dimensions.Length.Should().Be(product.Dimensions.Length);
        Repository.First().Dimensions.Width.Should().Be(request.Dimensions.Width);
        Repository.First().Dimensions.Height.Should().Be(request.Dimensions.Height);
    }

    [Fact]
    public async Task UpdateProductAsync_WithValidFieldMask_ShouldUpdateSpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        Repository.Add(product);
        Repository.IsDirty = false;
        UpdateProductRequest request = new()
        {
            Name = "Updated Name", Pricing = new ProductPricingRequest { BasePrice = 1.99m }, FieldMask = ["name"]
        };
        ICommandHandler<UpdateProductCommand> sut = UpdateProductService();

        await sut.Handle(new UpdateProductCommand { Product = product, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Name.Value.Should().Be(request.Name);
    }

    [Fact]
    public async Task UpdateProductAsync_GroomingAndHygiene_ShouldUpdateSpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithCategory(Category.GroomingAndHygiene).Build();
        Repository.Add(product);
        Repository.IsDirty = false;
        UpdateProductRequest request = new()
        {
            UsageInstructions = "Place into palm and apply",
            IsNatural = false,
            IsHypoAllergenic = true,
            IsCrueltyFree = false,
            SafetyWarnings = "Just don't strangle the dog",
            FieldMask = ["usageinstructions", "isnatural", "ishypoallergenic", "iscrueltyfree", "safetywarnings"]
        };
        ICommandHandler<UpdateProductCommand> sut = UpdateProductService();

        await sut.Handle(new UpdateProductCommand { Product = product, Request = request });

        Repository.IsDirty.Should().BeTrue();
        var groomingAndHygiene = (GroomingAndHygiene)Repository.First();
        groomingAndHygiene.UsageInstructions.Should().Be(request.UsageInstructions);
        groomingAndHygiene.IsNatural.Should().Be((bool)request.IsNatural);
        groomingAndHygiene.IsHypoallergenic.Should().Be((bool)request.IsHypoAllergenic);
        groomingAndHygiene.SafetyWarnings.Should().Be(request.SafetyWarnings);
        groomingAndHygiene.IsCrueltyFree.Should().Be((bool)request.IsCrueltyFree);
    }
}
