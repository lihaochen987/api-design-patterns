using AutoFixture;
using backend.Product.DomainModels.Views;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class GetProductPricingControllerTests : GetProductPricingControllerTestBase
{
    [Fact]
    public async Task GetProductPricing_ReturnsPricing_WhenFieldMaskIsWildcard()
    {
        ProductPricingView product = new ProductPricingTestDataBuilder().Build();
        ProductRepository.Add(product);
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["*"])
            .Create();
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> actionResult = await sut.GetProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        GetProductPricingResponse? response =
            JsonConvert.DeserializeObject<GetProductPricingResponse>(contentResult.Value!.ToString()!);
        response.ShouldBeEquivalentTo(Extensions.ToGetProductPricingResponse(product.Pricing, product.Id));
    }

    [Fact]
    public async Task GetProductPricing_ReturnsNotFound_WhenProductDoesNotExist()
    {
        GetProductPricingRequest? request = Fixture.Create<GetProductPricingRequest>();
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> result = await sut.GetProductPricing(999, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProductPricing_ReturnsPartialPricing_WhenFieldMaskIsSpecified()
    {
        ProductPricingView product = new ProductPricingTestDataBuilder().Build();
        ProductRepository.Add(product);
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["BasePrice"])
            .Create();
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> actionResult = await sut.GetProductPricing(product.Id, request);

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
        ProductRepository.Add(product);
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, [])
            .Create();
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> actionResult = await sut.GetProductPricing(product.Id, request);

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
        ProductRepository.Add(product);
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["InvalidField", "BasePrice", "TaxRate"])
            .Create();
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> actionResult = await sut.GetProductPricing(product.Id, request);

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
