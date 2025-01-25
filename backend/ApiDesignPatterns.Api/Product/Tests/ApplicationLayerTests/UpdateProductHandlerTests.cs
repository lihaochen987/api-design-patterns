// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared;
using backend.Shared.CommandHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class UpdateProductHandlerTests : UpdateProductHandlerTestBase
{
    [Fact]
    public async Task UpdateProductAsync_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithId(3).WithName("Original Name")
            .WithPricing(new Pricing(20.99m, 5m, 3m))
            .WithCategory(Category.Feeders).Build();
        Repository.Add(product);
        Repository.IsDirty = false;
        UpdateProductRequest request = new()
        {
            Name = "Updated Name",
            Pricing = new ProductPricingRequest { BasePrice = "25.50", DiscountPercentage = "50", TaxRate = "15" },
            Category = "Toys",
            FieldMask = ["name", "category", "discountpercentage", "taxrate"]
        };
        ICommandHandler<UpdateProductQuery> sut = UpdateProductService();

        await sut.Handle(new UpdateProductQuery { Product = product, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
        Repository.First().Name.ShouldBeEquivalentTo(request.Name);
        Repository.First().Category.ShouldBeEquivalentTo((Category)Enum.Parse(typeof(Category), request.Category));
        Repository.First().Pricing.DiscountPercentage
            .ShouldBeEquivalentTo(decimal.Parse(request.Pricing.DiscountPercentage));
        Repository.First().Pricing.TaxRate.ShouldBeEquivalentTo(decimal.Parse(request.Pricing.TaxRate));
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
            Dimensions = new DimensionsRequest { Length = "20", Width = "10", Height = "2" },
            FieldMask = ["dimensions.width", "dimensions.height"]
        };
        ICommandHandler<UpdateProductQuery> sut = UpdateProductService();

        await sut.Handle(new UpdateProductQuery { Product = product, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
        Repository.First().Dimensions.Length.ShouldBeEquivalentTo(product.Dimensions.Length);
        Repository.First().Dimensions.Width.ShouldBeEquivalentTo(decimal.Parse(request.Dimensions.Width));
        Repository.First().Dimensions.Height.ShouldBeEquivalentTo(decimal.Parse(request.Dimensions.Height));
    }

    [Fact]
    public async Task UpdateProductAsync_WithValidFieldMask_ShouldUpdateSpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        Repository.Add(product);
        Repository.IsDirty = false;
        UpdateProductRequest request = new()
        {
            Name = "Updated Name", Pricing = new ProductPricingRequest { BasePrice = "1.99" }, FieldMask = ["name"]
        };
        ICommandHandler<UpdateProductQuery> sut = UpdateProductService();

        await sut.Handle(new UpdateProductQuery { Product = product, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
        Repository.First().Name.ShouldBeEquivalentTo(request.Name);
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
        ICommandHandler<UpdateProductQuery> sut = UpdateProductService();

        await sut.Handle(new UpdateProductQuery { Product = product, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
        var groomingAndHygiene = (GroomingAndHygiene)Repository.First();
        groomingAndHygiene.UsageInstructions.ShouldBeEquivalentTo(request.UsageInstructions);
        groomingAndHygiene.IsNatural.ShouldBeEquivalentTo(request.IsNatural);
        groomingAndHygiene.IsHypoallergenic.ShouldBeEquivalentTo(request.IsHypoAllergenic);
        groomingAndHygiene.SafetyWarnings.ShouldBeEquivalentTo(request.SafetyWarnings);
        groomingAndHygiene.IsCrueltyFree.ShouldBeEquivalentTo(request.IsCrueltyFree);
    }
}
