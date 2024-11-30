using AutoFixture;
using backend.Product.FieldMasks;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class UpdateProductPricingControllerTests
{
    private readonly UpdateProductPricingController _controller;
    private readonly UpdateProductPricingExtensions _extensions;
    private readonly Fixture _fixture = new();
    private readonly ProductRepositoryFake _productRepository = [];

    public UpdateProductPricingControllerTests()
    {
        ProductPricingFieldMaskConfiguration configuration = new();
        _extensions = new UpdateProductPricingExtensions();
        _controller = new UpdateProductPricingController(
            _productRepository,
            configuration,
            _extensions);
    }

    [Fact]
    public async Task UpdateProductPricing_ReturnsNotFound_WhenProductDoesNotExist()
    {
        long id = _fixture.Create<long>();
        UpdateProductPricingRequest? request = _fixture.Create<UpdateProductPricingRequest>();

        ActionResult<UpdateProductPricingResponse> actionResult = await _controller.UpdateProductPricing(id, request);

        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProductPricing_UpdatesPricing_WhenProductExists()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        _productRepository.Add(product);
        _productRepository.IsDirty = false;

        UpdateProductPricingRequest request = new()
        {
            BasePrice = "99.99",
            DiscountPercentage = "10",
            TaxRate = "5",
            FieldMask = ["baseprice", "discountpercentage", "taxrate"]
        };

        ActionResult<UpdateProductPricingResponse> actionResult =
            await _controller.UpdateProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        UpdateProductPricingResponse? response = contentResult.Value as UpdateProductPricingResponse;
        response.ShouldBeEquivalentTo(_extensions.ToUpdateProductPricingResponse(product.Pricing, product.Id));
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }
}
