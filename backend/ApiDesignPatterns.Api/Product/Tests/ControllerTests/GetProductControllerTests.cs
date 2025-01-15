using System.Net;
using AutoFixture;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class GetProductControllerTests : GetProductControllerTestBase
{
    [Fact]
    public async Task GetProduct_ReturnsOkResult_WhenProductExists()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Dog Food").WithCategory(Category.PetFood)
            .Build();
        GetProductRequest request = Fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name", "Price"])
            .Create();
        Mock
            .Get(MockQueryApplicationService)
            .Setup(service => service.GetProductView(productView.Id))
            .ReturnsAsync(productView);
        var sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productView.Id, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Dog Food");
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        ProductView productView = new ProductViewTestDataBuilder().Build();
        GetProductRequest request = Fixture.Create<GetProductRequest>();
        Mock
            .Get(MockQueryApplicationService)
            .Setup(service => service.GetProductView(productView.Id))
            .ReturnsAsync((ProductView?)null);
        var sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productView.Id, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }


    [Fact]
    public async Task
        GetProduct_ReturnsOkResult_WithGroomingAndHygieneResponse_WhenProductCategoryIsGroomingAndHygiene()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Shampoo")
            .WithCategory(Category.GroomingAndHygiene).Build();
        GetProductRequest request = Fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name", "Price"])
            .Create();
        Mock
            .Get(MockQueryApplicationService)
            .Setup(service => service.GetProductView(productView.Id))
            .ReturnsAsync(productView);
        var sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productView.Id, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Shampoo");
    }

    [Fact]
    public async Task GetProduct_ReturnsOkResult_WithGenericResponse_WhenProductCategoryIsNotPetFoodOrGrooming()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Other Product").WithCategory(Category.Beds)
            .Build();
        GetProductRequest request = Fixture.Create<GetProductRequest>();
        Mock
            .Get(MockQueryApplicationService)
            .Setup(service => service.GetProductView(productView.Id))
            .ReturnsAsync(productView);
        var sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productView.Id, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Other Product");
    }


    [Fact]
    public async Task GetProduct_SerializesWithFieldMaskCorrectly()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Masked Product").Build();
        GetProductRequest request = Fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name"])
            .Create();
        Mock
            .Get(MockQueryApplicationService)
            .Setup(service => service.GetProductView(productView.Id))
            .ReturnsAsync(productView);
        var sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productView.Id, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Masked Product");
        jsonResult.ShouldNotContain("Price");
    }
}
