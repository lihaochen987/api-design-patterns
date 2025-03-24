using System.Net;
using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class GetProductPricingControllerTests : GetProductPricingControllerTestBase
{
    [Fact]
    public async Task GetProductPricing_ReturnsPricing_WhenFieldMaskIsWildcard()
    {
        ProductPricingView product = new ProductPricingViewTestDataBuilder().Build();
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["*"])
            .Create();
        Mock
            .Get(MockGetProductPricing)
            .Setup(service => service.Handle(It.Is<GetProductPricingQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> actionResult = await sut.GetProductPricing(product.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult? contentResult = (OkObjectResult)actionResult.Result;
        GetProductPricingResponse response =
            JsonConvert.DeserializeObject<GetProductPricingResponse>(contentResult.Value!.ToString()!)!;
        response.pricing.Should().BeEquivalentTo(Mapper.Map<ProductPricingResponse>(product.Pricing));
    }

    [Fact]
    public async Task GetProductPricing_ReturnsNotFound_WhenProductDoesNotExist()
    {
        GetProductPricingRequest? request = Fixture.Create<GetProductPricingRequest>();
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> result = await sut.GetProductPricing(999, request);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetProductPricing_ReturnsPartialPricing_WhenFieldMaskIsSpecified()
    {
        ProductPricingView product = new ProductPricingViewTestDataBuilder().Build();
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["pricing.BasePrice"])
            .Create();
        Mock
            .Get(MockGetProductPricing)
            .Setup(service => service.Handle(It.Is<GetProductPricingQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> result = await sut.GetProductPricing(product.Id, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("BasePrice");
    }

    [Fact]
    public async Task GetProductPricing_ReturnsAllFields_WhenFieldMaskIsEmpty()
    {
        ProductPricingView product = new ProductPricingViewTestDataBuilder().Build();
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, [])
            .Create();
        Mock
            .Get(MockGetProductPricing)
            .Setup(service => service.Handle(It.Is<GetProductPricingQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> result = await sut.GetProductPricing(product.Id, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("BasePrice");
        jsonResult.Should().Contain("DiscountPercentage");
        jsonResult.Should().Contain("TaxRate");
    }

    [Fact]
    public async Task GetProductPricing_ReturnsValidMasks_WhenInvalidMasksArePassed()
    {
        ProductPricingView product = new ProductPricingViewTestDataBuilder().Build();
        GetProductPricingRequest? request = Fixture.Build<GetProductPricingRequest>()
            .With(r => r.FieldMask, ["InvalidField", "pricing.baseprice", "pricing.taxrate"])
            .Create();
        Mock
            .Get(MockGetProductPricing)
            .Setup(service => service.Handle(It.Is<GetProductPricingQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = ProductPricingController();

        ActionResult<GetProductPricingResponse> result = await sut.GetProductPricing(product.Id, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("BasePrice");
        jsonResult.Should().Contain("TaxRate");
        jsonResult.Should().NotContain("DiscountPercentage");
    }
}
