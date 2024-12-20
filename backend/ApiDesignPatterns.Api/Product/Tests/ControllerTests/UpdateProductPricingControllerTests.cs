using AutoFixture;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class UpdateProductPricingControllerTests : UpdateProductPricingControllerTestBase
{
    [Fact]
    public async Task UpdateProductPricing_ReturnsNotFound_WhenProductDoesNotExist()
    {
        long id = Fixture.Create<long>();
        UpdateProductPricingRequest? request = Fixture.Create<UpdateProductPricingRequest>();
        var sut = UpdateProductPricingController();

        ActionResult<UpdateProductPricingResponse> actionResult = await sut.UpdateProductPricing(id, request);

        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProductPricing_UpdatesPricing_WhenProductExists()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        ProductRepository.Add(product);
        ProductRepository.IsDirty = false;
        UpdateProductPricingRequest request = new()
        {
            BasePrice = "99.99",
            DiscountPercentage = "10",
            TaxRate = "5",
            FieldMask = ["baseprice", "discountpercentage", "taxrate"]
        };
        var sut = UpdateProductPricingController();

        ActionResult<UpdateProductPricingResponse> actionResult = await sut.UpdateProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        UpdateProductPricingResponse? response = contentResult.Value as UpdateProductPricingResponse;
        response.ShouldBeEquivalentTo(Extensions.ToUpdateProductPricingResponse(product.Pricing, product.Id));
        ProductRepository.IsDirty.ShouldBeEquivalentTo(true);
    }
}
