// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ProductApplicationServiceTests : ProductApplicationServiceTestBase
{
    [Fact]
    public async Task GetProductAsync_ReturnsProduct_WhenProductExists()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        Repository.Add(expectedProduct);
        ProductApplicationService sut = ProductApplicationService();

        DomainModels.Product? result = await sut.GetProductAsync(expectedProduct.Id);

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task GetProductAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        ProductApplicationService sut = ProductApplicationService();

        DomainModels.Product? result = await sut.GetProductAsync(expectedProduct.Id);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task CreateProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToCreate = new ProductTestDataBuilder().Build();
        ProductApplicationService sut = ProductApplicationService();

        await sut.CreateProductAsync(productToCreate);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 1);
    }

    [Fact]
    public async Task CreateProductAsync_PersistsWhenCalledTwice()
    {
        DomainModels.Product firstProductToCreate = new ProductTestDataBuilder().Build();
        DomainModels.Product secondProductToCreate = new ProductTestDataBuilder().Build();
        ProductApplicationService sut = ProductApplicationService();

        await sut.CreateProductAsync(firstProductToCreate);
        await sut.CreateProductAsync(secondProductToCreate);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 2);
    }

    [Fact]
    public async Task DeleteProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToDelete = new ProductTestDataBuilder().Build();
        ProductApplicationService sut = ProductApplicationService();

        await sut.DeleteProductAsync(productToDelete);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("DeleteProductAsync", 1);
    }

    [Fact]
    public async Task UpdateProductAsync_ValidProduct_ShouldCallRepositoryUpdateProductAsync()
    {
        var existingProduct = new ProductTestDataBuilder().WithId(1).Build();
        Repository.Add(existingProduct);
        Repository.IsDirty = false;
        var updatedProduct = new ProductTestDataBuilder().WithId(1).Build();
        ProductApplicationService sut = ProductApplicationService();

        await sut.ReplaceProductAsync(updatedProduct);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
    }

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
            Pricing = new ProductPricingRequest { BasePrice = "25.50", DiscountPercentage = "50" },
            Category = "Toys",
            FieldMask = ["name", "category", "discountpercentage"]
        };
        ProductApplicationService sut = ProductApplicationService();

        await sut.UpdateProductAsync(request, product);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
        Repository.First().Name.ShouldBeEquivalentTo(request.Name);
        Repository.First().Category.ShouldBeEquivalentTo((Category)Enum.Parse(typeof(Category), request.Category));
        Repository.First().Pricing.DiscountPercentage
            .ShouldBeEquivalentTo(decimal.Parse(request.Pricing.DiscountPercentage));
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
        ProductApplicationService sut = ProductApplicationService();

        await sut.UpdateProductAsync(request, product);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
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
        ProductApplicationService sut = ProductApplicationService();

        await sut.UpdateProductAsync(request, product);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
        Repository.First().Name.ShouldBeEquivalentTo(request.Name);
    }
}
