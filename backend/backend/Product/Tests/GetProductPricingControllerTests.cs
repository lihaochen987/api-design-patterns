using AutoFixture;
using backend.Product.FieldMasks;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class GetProductPricingControllerTests
{
    private readonly Fixture _fixture = new();
    private readonly GetProductPricingController _controller;
    private readonly GetProductPricingExtensions _extensions;
    private readonly ProductPricingRepositoryFake _productRepository = [];

    public GetProductPricingControllerTests()
    {
        var configuration = new ProductPricingFieldMaskConfiguration();
        _extensions = new GetProductPricingExtensions();
        _controller = new GetProductPricingController(
            _productRepository,
            configuration,
            _extensions);
    }

    [Fact]
    public async Task GetProductPricing_ReturnsPricing_WhenFieldMaskIsWildcard()
    {
        var product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        var request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["*"])
            .Create();

        var actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = JsonConvert.DeserializeObject<GetProductPricingResponse>(contentResult.Value!.ToString()!);
        response.ShouldBeEquivalentTo(_extensions.ToGetProductPricingResponse(product.Pricing, product.Id));
    }

    [Fact]
    public async Task GetProductPricing_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var request = _fixture.Create<GetProductPricingRequest>();

        var result = await _controller.GetProductPricing(999, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProductPricing_ReturnsPartialPricing_WhenFieldMaskIsSpecified()
    {
        var product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        var request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["BasePrice"])
            .Create();

        var actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var result = actionResult.Result as OkObjectResult;
        var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
        response!.ShouldContainKey("BasePrice");
        response!.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetProductPricing_ReturnsAllFields_WhenFieldMaskIsEmpty()
    {
        var product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        var request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, [])
            .Create();

        var actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var result = actionResult.Result as OkObjectResult;
        var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(result!.Value!.ToString()!);
        response!.ShouldContainKey("BasePrice");
        response!.ShouldContainKey("DiscountPercentage");
        response!.ShouldContainKey("TaxRate");
    }

    [Fact]
    public async Task GetProductPricing_ReturnsValidMasks_WhenInvalidMasksArePassed()
    {
        var product = new ProductPricingTestDataBuilder().Build();
        _productRepository.Add(product);

        var request = _fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["InvalidField", "BasePrice", "TaxRate"])
            .Create();

        var actionResult = await _controller.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        var contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentResult.Value!.ToString()!);
        response!.ShouldContainKey("BasePrice");
        response!.ShouldContainKey("TaxRate");
        response!.Count.ShouldBe(2);
    }
}