using System.Net;
using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("Dog Food");
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

        result.Result.Should().BeOfType<NotFoundResult>();
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

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("Shampoo");
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

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("Other Product");
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

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("Masked Product");
        jsonResult.Should().NotContain("Price");
    }
}
