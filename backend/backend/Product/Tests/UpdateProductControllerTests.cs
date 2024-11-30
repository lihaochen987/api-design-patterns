using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.FieldMasks;
using backend.Product.ProductControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class UpdateProductControllerTests
{
    private readonly UpdateProductController _controller;
    private readonly UpdateProductExtensions _extensions;
    private readonly ProductRepositoryFake _productRepository = [];

    public UpdateProductControllerTests()
    {
        var configuration = new ProductFieldMaskConfiguration();
        _extensions = new UpdateProductExtensions(new TypeParser());
        _controller = new UpdateProductController(
            _productRepository,
            configuration,
            _extensions);
    }

    [Fact]
    public async Task UpdateProduct_WithEmptyFieldMask_ShouldUpdateNoFields()
    {
        var product = new ProductTestDataBuilder().Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            Pricing = new ProductPricingContract { BasePrice = "1.99" },
            Category = "Toys"
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response.ShouldBeEquivalentTo(_extensions.ToUpdateProductResponse(product));
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task UpdateProduct_WithValidFieldMask_ShouldUpdateSpecifiedFields()
    {
        var product = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            Pricing = new ProductPricingContract { BasePrice = "1.99" },
            FieldMask = ["name"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response!.Name.ShouldBeEquivalentTo(request.Name);
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task UpdateProduct_NonExistentProduct_ShouldReturnNotFound()
    {
        var request = new UpdateProductRequest
        {
            Name = "Non-Existent Product",
            FieldMask = ["name"]
        };
        const int nonExistentId = 999;

        var actionResult = await _controller.UpdateProduct(nonExistentId, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProduct_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var product = new ProductTestDataBuilder().WithId(3).WithName("Original Name")
            .WithPricing(new Pricing(20.99m, 5m, 3m))
            .WithCategory(Category.Feeders).Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        var request = new UpdateProductRequest
        {
            Name = "Updated Name",
            Pricing = new ProductPricingContract { BasePrice = "25.50", DiscountPercentage = "50" },
            Category = "Toys",
            FieldMask = ["name", "category", "discountpercentage"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        var response = Assert.IsType<OkObjectResult>(actionResult.Result).Value as UpdateProductResponse;
        response.ShouldNotBeNull();
        response.Name.ShouldBeEquivalentTo(request.Name);
        response.Category.ShouldBeEquivalentTo(request.Category);
        response.Pricing.DiscountPercentage.ShouldBeEquivalentTo(request.Pricing.DiscountPercentage);
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task UpdateProduct_WithNestedFieldInFieldMask_ShouldUpdateNestedField()
    {
        var product = new ProductTestDataBuilder()
            .WithId(5).WithDimensions(new Dimensions(10, 5, 2)).Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        var request = new UpdateProductRequest
        {
            Dimensions = new DimensionsContract { Length = "20", Width = "10", Height = "2" },
            FieldMask = ["dimensions.width", "dimensions.height"]
        };

        var actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = contentResult.Value as UpdateProductResponse;
        response!.Dimensions.Length.ShouldBe(product.Dimensions.Length.ToString(CultureInfo.InvariantCulture));
        response.Dimensions.Width.ShouldBe(request.Dimensions.Width);
        response.Dimensions.Height.ShouldBe(request.Dimensions.Height);
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }
}