using System.Globalization;
using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class UpdateProductControllerTests
{
    private readonly UpdateProductController _controller;
    private readonly IMapper _mapper;
    private readonly ProductRepositoryFake _productRepository = [];

    public UpdateProductControllerTests()
    {
        MapperConfiguration? mapperConfiguration = new(cfg => { cfg.AddProfile<UpdateProductMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
        ProductFieldMaskConfiguration configuration = new();
        _controller = new UpdateProductController(
            _productRepository,
            configuration,
            _mapper);
    }

    [Fact]
    public async Task UpdateProduct_WithEmptyFieldMask_ShouldUpdateNoFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        UpdateProductRequest request = new()
        {
            Name = "Updated Name", Pricing = new ProductPricingContract { BasePrice = "1.99" }, Category = "Toys"
        };

        ActionResult<UpdateProductResponse> actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        UpdateProductResponse? response = contentResult.Value as UpdateProductResponse;
        response.ShouldBeEquivalentTo(_mapper.Map<UpdateProductResponse>(product));
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task UpdateProduct_WithValidFieldMask_ShouldUpdateSpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        UpdateProductRequest request = new()
        {
            Name = "Updated Name", Pricing = new ProductPricingContract { BasePrice = "1.99" }, FieldMask = ["name"]
        };

        ActionResult<UpdateProductResponse> actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        UpdateProductResponse? response = contentResult.Value as UpdateProductResponse;
        response!.Name.ShouldBeEquivalentTo(request.Name);
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task UpdateProduct_NonExistentProduct_ShouldReturnNotFound()
    {
        UpdateProductRequest request = new() { Name = "Non-Existent Product", FieldMask = ["name"] };
        const int nonExistentId = 999;

        ActionResult<UpdateProductResponse> actionResult = await _controller.UpdateProduct(nonExistentId, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProduct_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithId(3).WithName("Original Name")
            .WithPricing(new Pricing(20.99m, 5m, 3m))
            .WithCategory(Category.Feeders).Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        UpdateProductRequest request = new()
        {
            Name = "Updated Name",
            Pricing = new ProductPricingContract { BasePrice = "25.50", DiscountPercentage = "50" },
            Category = "Toys",
            FieldMask = ["name", "category", "discountpercentage"]
        };

        ActionResult<UpdateProductResponse> actionResult = await _controller.UpdateProduct(product.Id, request);

        UpdateProductResponse? response =
            Assert.IsType<OkObjectResult>(actionResult.Result).Value as UpdateProductResponse;
        response.ShouldNotBeNull();
        response.Name.ShouldBeEquivalentTo(request.Name);
        response.Category.ShouldBeEquivalentTo(request.Category);
        response.Pricing.DiscountPercentage.ShouldBeEquivalentTo(request.Pricing.DiscountPercentage);
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async Task UpdateProduct_WithNestedFieldInFieldMask_ShouldUpdateNestedField()
    {
        DomainModels.Product product = new ProductTestDataBuilder()
            .WithId(5).WithDimensions(new Dimensions(10, 5, 2)).Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        UpdateProductRequest request = new()
        {
            Dimensions = new DimensionsContract { Length = "20", Width = "10", Height = "2" },
            FieldMask = ["dimensions.width", "dimensions.height"]
        };

        ActionResult<UpdateProductResponse> actionResult = await _controller.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        UpdateProductResponse? response = contentResult.Value as UpdateProductResponse;
        response!.Dimensions.Length.ShouldBe(product.Dimensions.Length.ToString(CultureInfo.InvariantCulture));
        response.Dimensions.Width.ShouldBe(request.Dimensions.Width);
        response.Dimensions.Height.ShouldBe(request.Dimensions.Height);
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }
}
