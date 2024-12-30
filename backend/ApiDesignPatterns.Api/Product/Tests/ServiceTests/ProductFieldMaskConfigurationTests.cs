// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Product.Tests.TestHelpers.Builders;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ServiceTests;

public class ProductFieldMaskConfigurationTests : ProductFieldMaskConfigurationTestBase
{
    [Fact]
    public void GetUpdatedProductValues_ShouldUpdateName_WhenFieldMaskContainsName()
    {
        var baseProduct = new ProductTestDataBuilder().WithName("Original Product").Build();
        var request = new UpdateProductRequest { Name = "Updated Product", FieldMask = ["name"] };
        ProductFieldMaskConfiguration sut = ProductFieldMaskConfiguration();

        (string name, Pricing pricing, Category category, Dimensions dimensions) result =
            sut.GetUpdatedProductValues(request, baseProduct);

        result.name.ShouldBeEquivalentTo(request.Name);
        result.category.ShouldBeEquivalentTo(baseProduct.Category);
        result.dimensions.ShouldBeEquivalentTo(baseProduct.Dimensions);
    }

    [Fact]
    public void GetUpdatedProductValues_ShouldNotUpdateName_WhenFieldMaskExcludesName()
    {
        var baseProduct = new ProductTestDataBuilder().WithName("Original Product").Build();
        var request = new UpdateProductRequest { Name = "Updated Product", FieldMask = ["category"] };
        ProductFieldMaskConfiguration sut = ProductFieldMaskConfiguration();

        (string name, Pricing pricing, Category category, Dimensions dimensions) result =
            sut.GetUpdatedProductValues(request, baseProduct);

        result.name.ShouldBeEquivalentTo(baseProduct.Name);
    }

    [Fact]
    public void GetUpdatedDimensionValues_ShouldUpdateLength_WhenFieldMaskContainsLength()
    {
        var baseDimensions = Fixture.Create<Dimensions>();
        var request = new UpdateProductRequest
        {
            FieldMask = ["dimensions.length"], Dimensions = new DimensionsRequest { Length = "40" }
        };
        ProductFieldMaskConfiguration sut = ProductFieldMaskConfiguration();

        Dimensions result = sut.GetUpdatedDimensionValues(request, baseDimensions);

        result.Length.ShouldBeEquivalentTo(40m);
        result.Width.ShouldBeEquivalentTo(baseDimensions.Width);
        result.Height.ShouldBeEquivalentTo(baseDimensions.Height);
    }

    [Fact]
    public void GetUpdatedProductPricingValues_ShouldUpdateBasePrice_WhenFieldMaskContainsBasePrice()
    {
        var basePricing = new ProductPricingTestDataBuilder().Build();
        var request = new UpdateProductRequest
        {
            FieldMask = ["baseprice"], Pricing = new ProductPricingRequest { BasePrice = "200" }
        };
        ProductFieldMaskConfiguration sut = ProductFieldMaskConfiguration();

        Pricing result = sut.GetUpdatedProductPricingValues(request, basePricing);

        result.BasePrice.ShouldBeEquivalentTo(200m);
        result.DiscountPercentage.ShouldBeEquivalentTo(basePricing.DiscountPercentage);
        result.TaxRate.ShouldBeEquivalentTo(basePricing.TaxRate);
    }
    //
    // [Theory, AutoData]
    // public void GetUpdatedPetFoodValues_ShouldUpdateAgeGroup_WhenFieldMaskContainsAgeGroup(UpdateProductRequest request)
    // {
    //     // Arrange
    //     var petFood = _fixture.Build<PetFood>()
    //         .With(p => p.AgeGroup, AgeGroup.Adult)
    //         .Create();
    //     request.FieldMask = new HashSet<string> { "agegroup" };
    //     request.AgeGroup = "Puppy";
    //
    //     // Act
    //     var result = _configuration.GetUpdatedPetFoodValues(request, petFood);
    //
    //     // Assert
    //     Assert.Equal(AgeGroup.Puppy, result.ageGroup);
    //     Assert.Equal(petFood.BreedSize, result.breedSize);
    // }
    //
    // [Theory, AutoData]
    // public void GetUpdatedGroomingAndHygieneValues_ShouldUpdateIsCrueltyFree_WhenFieldMaskContainsIsCrueltyFree(
    //     UpdateProductRequest request)
    // {
    //     // Arrange
    //     var groomingAndHygiene = _fixture.Build<GroomingAndHygiene>()
    //         .With(g => g.IsCrueltyFree, false)
    //         .Create();
    //     request.FieldMask = new HashSet<string> { "iscrueltyfree" };
    //     request.IsCrueltyFree = true;
    //
    //     // Act
    //     var result = _configuration.GetUpdatedGroomingAndHygieneValues(request, groomingAndHygiene);
    //
    //     // Assert
    //     Assert.True(result.isCrueltyFree);
    // }
}
