using System.Net;
using AutoFixture;
using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.ProductControllers;
using backend.Product.Services;
using backend.Product.Tests.Helpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.Controllers;

public class GetProductControllerTests
{
    private readonly GetProductController _controller;
    private readonly Fixture _fixture = new();
    private readonly IProductViewApplicationService _mockApplicationService = Mock.Of<IProductViewApplicationService>();

    public GetProductControllerTests()
    {
        MapperConfiguration? mapperConfiguration = new(cfg => { cfg.AddProfile<GetProductMappingProfile>(); });
        IMapper mapper = mapperConfiguration.CreateMapper();
        ProductFieldMaskConfiguration configuration = new();
        _controller = new GetProductController(
            _mockApplicationService,
            configuration,
            mapper);
    }

    [Fact]
    public async Task GetProduct_ReturnsOkResult_WhenProductExists()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Dog Food").WithCategory(Category.PetFood)
            .Build();
        GetProductRequest request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name", "Price"])
            .Create();
        Mock
            .Get(_mockApplicationService)
            .Setup(service => service.GetProductView(productView.Id, request))
            .ReturnsAsync(productView);

        ActionResult<GetProductResponse>? result = await _controller.GetProduct(productView.Id, request);

        OkObjectResult? okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Dog Food");
        Mock
            .Get(_mockApplicationService)
            .Verify(service => service.GetProductView(productView.Id, request), Times.Once);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        ProductView productView = new ProductViewTestDataBuilder().Build();
        GetProductRequest request = _fixture.Create<GetProductRequest>();
        Mock
            .Get(_mockApplicationService)
            .Setup(service => service.GetProductView(productView.Id, request))
            .ReturnsAsync((ProductView?)null);

        ActionResult<GetProductResponse>? result = await _controller.GetProduct(productView.Id, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(_mockApplicationService)
            .Verify(service => service.GetProductView(productView.Id, request), Times.Once);
    }


    [Fact]
    public async Task
        GetProduct_ReturnsOkResult_WithGroomingAndHygieneResponse_WhenProductCategoryIsGroomingAndHygiene()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Shampoo")
            .WithCategory(Category.GroomingAndHygiene).Build();
        GetProductRequest request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name", "Price"])
            .Create();
        Mock
            .Get(_mockApplicationService)
            .Setup(service => service.GetProductView(productView.Id, request))
            .ReturnsAsync(productView);

        ActionResult<GetProductResponse>? result = await _controller.GetProduct(productView.Id, request);

        OkObjectResult? okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Shampoo");
        Mock
            .Get(_mockApplicationService)
            .Verify(service => service.GetProductView(productView.Id, request), Times.Once);
    }

    [Fact]
    public async Task GetProduct_ReturnsOkResult_WithGenericResponse_WhenProductCategoryIsNotPetFoodOrGrooming()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Other Product").WithCategory(Category.Beds)
            .Build();
        GetProductRequest request = _fixture.Create<GetProductRequest>();
        Mock
            .Get(_mockApplicationService)
            .Setup(service => service.GetProductView(productView.Id, request))
            .ReturnsAsync(productView);

        ActionResult<GetProductResponse>? result = await _controller.GetProduct(productView.Id, request);

        // Assert
        OkObjectResult? okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Other Product");
        Mock
            .Get(_mockApplicationService)
            .Verify(service => service.GetProductView(productView.Id, request), Times.Once);
    }


    [Fact]
    public async Task GetProduct_SerializesWithFieldMaskCorrectly()
    {
        ProductView productView = new ProductViewTestDataBuilder().WithName("Masked Product").Build();
        GetProductRequest request = _fixture.Build<GetProductRequest>()
            .With(r => r.FieldMask, ["Name"])
            .Create();
        Mock
            .Get(_mockApplicationService)
            .Setup(service => service.GetProductView(productView.Id, request))
            .ReturnsAsync(productView);

        ActionResult<GetProductResponse>? result = await _controller.GetProduct(productView.Id, request);

        OkObjectResult? okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("Masked Product");
        jsonResult.ShouldNotContain("Price");
        Mock
            .Get(_mockApplicationService)
            .Verify(service => service.GetProductView(productView.Id, request), Times.Once);
    }
}
