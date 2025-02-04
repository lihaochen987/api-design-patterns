using System.Net;
using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.DomainModels.Enums;
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
        long productId = Fixture.Create<long>();
        GetProductResponse productResponse = new ProductViewTestDataBuilder()
            .WithId(productId)
            .WithName("Dog Food")
            .WithCategory(Category.PetFood)
            .BuildAndConvertToResponse();
        GetProductRequest request = Fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name", "Price"])
            .Create();
        Mock
            .Get(MockGetProductResponse)
            .Setup(service =>
                service.Handle(It.Is<GetProductResponseQuery>(q => q.Id.ToString() == productResponse.Id)))
            .ReturnsAsync(productResponse);
        GetProductController sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Dog Food");
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        long productId = Fixture.Create<long>();
        GetProductRequest request = Fixture.Create<GetProductRequest>();
        Mock
            .Get(MockGetProductResponse)
            .Setup(service => service.Handle(It.Is<GetProductResponseQuery>(q => q.Id == productId)))
            .ReturnsAsync((GetProductResponse?)null);
        GetProductController sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productId, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }


    [Fact]
    public async Task
        GetProduct_ReturnsOkResult_WithGroomingAndHygieneResponse_WhenProductCategoryIsGroomingAndHygiene()
    {
        long productId = Fixture.Create<long>();
        GetProductResponse productResponse = new ProductViewTestDataBuilder()
            .WithId(productId)
            .WithName("Shampoo")
            .WithCategory(Category.GroomingAndHygiene)
            .BuildAndConvertToResponse();
        GetProductRequest request = Fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name", "Price"])
            .Create();
        Mock
            .Get(MockGetProductResponse)
            .Setup(service => service.Handle(It.Is<GetProductResponseQuery>(q => q.Id == productId)))
            .ReturnsAsync(productResponse);
        GetProductController sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Shampoo");
    }

    [Fact]
    public async Task GetProduct_ReturnsOkResult_WithGenericResponse_WhenProductCategoryIsNotPetFoodOrGrooming()
    {
        long productId = Fixture.Create<long>();
        GetProductResponse productResponse = new ProductViewTestDataBuilder()
            .WithId(productId)
            .WithName("Other Product")
            .WithCategory(Category.Beds)
            .BuildAndConvertToResponse();
        GetProductRequest request = Fixture.Create<GetProductRequest>();
        Mock
            .Get(MockGetProductResponse)
            .Setup(service => service.Handle(It.Is<GetProductResponseQuery>(q => q.Id == productId)))
            .ReturnsAsync(productResponse);
        GetProductController sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Other Product");
    }


    [Fact]
    public async Task GetProduct_SerializesWithFieldMaskCorrectly()
    {
        long productId = Fixture.Create<long>();
        GetProductResponse productResponse = new ProductViewTestDataBuilder()
            .WithId(productId)
            .WithName("Masked Product")
            .BuildAndConvertToResponse();
        GetProductRequest request = Fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name"])
            .Create();
        Mock
            .Get(MockGetProductResponse)
            .Setup(service => service.Handle(It.Is<GetProductResponseQuery>(q => q.Id == productId)))
            .ReturnsAsync(productResponse);
        GetProductController sut = GetProductController();

        ActionResult<GetProductResponse> result = await sut.GetProduct(productId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Masked Product");
        jsonResult.ShouldNotContain("Price");
    }
}
