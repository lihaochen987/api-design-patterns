using System.Globalization;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class UpdateProductControllerTests : UpdateProductControllerTestBase
{
    [Fact]
    public async Task UpdateProduct_WithEmptyFieldMask_ShouldUpdateNoFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        UpdateProductRequest request = new()
        {
            Name = "Updated Name", Pricing = new ProductPricingRequest { BasePrice = "1.99" }, Category = "Toys"
        };
        Mock
            .Get(MockApplicationService)
            .Setup(service => service.GetProductAsync(product.Id))
            .ReturnsAsync(product);
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        UpdateProductResponse? response = contentResult!.Value as UpdateProductResponse;
        response.ShouldBeEquivalentTo(Mapper.Map<UpdateProductResponse>(product));
        Mock
            .Get(MockApplicationService)
            .Verify(svc => svc.UpdateProductAsync(It.IsAny<DomainModels.Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_WithValidFieldMask_ShouldUpdateSpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        UpdateProductRequest request = new()
        {
            Name = "Updated Name", Pricing = new ProductPricingRequest { BasePrice = "1.99" }, FieldMask = ["name"]
        };
        Mock.Get(MockApplicationService).Setup(service => service.GetProductAsync(product.Id)).ReturnsAsync(product);
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        UpdateProductResponse? response = contentResult!.Value as UpdateProductResponse;
        response!.Name.ShouldBeEquivalentTo(request.Name);
        Mock
            .Get(MockApplicationService)
            .Verify(svc => svc.UpdateProductAsync(It.IsAny<DomainModels.Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_NonExistentProduct_ShouldReturnNotFound()
    {
        UpdateProductRequest request = new() { Name = "Non-Existent Product", FieldMask = ["name"] };
        const int nonExistentId = 999;
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(nonExistentId, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProduct_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().WithId(3).WithName("Original Name")
            .WithPricing(new Pricing(20.99m, 5m, 3m))
            .WithCategory(Category.Feeders).Build();
        UpdateProductRequest request = new()
        {
            Name = "Updated Name",
            Pricing = new ProductPricingRequest { BasePrice = "25.50", DiscountPercentage = "50" },
            Category = "Toys",
            FieldMask = ["name", "category", "discountpercentage"]
        };
        Mock.Get(MockApplicationService).Setup(service => service.GetProductAsync(product.Id)).ReturnsAsync(product);
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(product.Id, request);

        UpdateProductResponse? response =
            Assert.IsType<OkObjectResult>(actionResult.Result).Value as UpdateProductResponse;
        response!.Name.ShouldBeEquivalentTo(request.Name);
        response.Category.ShouldBeEquivalentTo(request.Category);
        response.Pricing.DiscountPercentage.ShouldBeEquivalentTo(request.Pricing.DiscountPercentage);
        Mock.Get(MockApplicationService)
            .Verify(svc => svc.UpdateProductAsync(It.IsAny<DomainModels.Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_WithNestedFieldInFieldMask_ShouldUpdateNestedField()
    {
        DomainModels.Product product = new ProductTestDataBuilder()
            .WithId(5).WithDimensions(new Dimensions(10, 5, 2)).Build();
        UpdateProductRequest request = new()
        {
            Dimensions = new DimensionsRequest { Length = "20", Width = "10", Height = "2" },
            FieldMask = ["dimensions.width", "dimensions.height"]
        };
        Mock.Get(MockApplicationService).Setup(service => service.GetProductAsync(product.Id)).ReturnsAsync(product);
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        UpdateProductResponse? response = contentResult!.Value as UpdateProductResponse;
        response!.Dimensions.Length.ShouldBe(product.Dimensions.Length.ToString(CultureInfo.InvariantCulture));
        response.Dimensions.Width.ShouldBe(request.Dimensions.Width);
        response.Dimensions.Height.ShouldBe(request.Dimensions.Height);
        Mock.Get(MockApplicationService)
            .Verify(svc => svc.UpdateProductAsync(It.IsAny<DomainModels.Product>()), Times.Once);
    }
}
