using AutoFixture;
using backend.Product.DomainModels.Views;
using backend.Product.ProductPricingControllers;
using backend.Product.Services;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class GetProductPricingControllerTests
{
    private readonly GetProductPricingController _controller;
    private readonly GetProductPricingExtensions _extensions;
    private readonly Fixture _fixture = new();
    private readonly ProductPricingRepositoryFake _productRepository = [];

    public GetProductPricingControllerTests()
    {
        ProductPricingFieldMaskConfiguration configuration = new();
        _extensions = new GetProductPricingExtensions();
        _controller = new GetProductPricingController(
            _productRepository,
            configuration,
            _extensions);
    }

    [Fact]
    public async Task GetProductPricing_ReturnsPricing_WhenFieldMaskIsWildcard()
    {
        ProductPricingView product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductPricingRequest? request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["*"])
            .Create();

        ActionResult<GetProductPricingResponse> actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        GetProductPricingResponse? response =
            JsonConvert.DeserializeObject<GetProductPricingResponse>(contentResult.Value!.ToString()!);
        response.ShouldBeEquivalentTo(_extensions.ToGetProductPricingResponse(product.Pricing, product.Id));
    }

    [Fact]
    public async Task GetProductPricing_ReturnsNotFound_WhenProductDoesNotExist()
    {
        GetProductPricingRequest? request = _fixture.Create<GetProductPricingRequest>();

        ActionResult<GetProductPricingResponse> result = await _controller.GetProductPricing(999, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProductPricing_ReturnsPartialPricing_WhenFieldMaskIsSpecified()
    {
        ProductPricingView product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductPricingRequest? request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["BasePrice"])
            .Create();

        ActionResult<GetProductPricingResponse> actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? result = actionResult.Result as OkObjectResult;
        Dictionary<string, object>? response =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
        response!.ShouldContainKey("BasePrice");
        response!.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetProductPricing_ReturnsAllFields_WhenFieldMaskIsEmpty()
    {
        ProductPricingView product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductPricingRequest? request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, [])
            .Create();

        ActionResult<GetProductPricingResponse> actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? result = actionResult.Result as OkObjectResult;
        Dictionary<string, object>? response =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
        response!.ShouldContainKey("BasePrice");
        response!.ShouldContainKey("DiscountPercentage");
        response!.ShouldContainKey("TaxRate");
    }

    [Fact]
    public async Task GetProductPricing_ReturnsValidMasks_WhenInvalidMasksArePassed()
    {
        ProductPricingView product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        GetProductPricingRequest? request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["InvalidField", "BasePrice", "TaxRate"])
            .Create();

        ActionResult<GetProductPricingResponse> actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        Dictionary<string, object>? response =
            JsonConvert.DeserializeObject<Dictionary<string, object>>(contentResult.Value!.ToString()!);
        response!.ShouldContainKey("BasePrice");
        response!.ShouldContainKey("TaxRate");
        response!.Count.ShouldBe(2);
    }
}
