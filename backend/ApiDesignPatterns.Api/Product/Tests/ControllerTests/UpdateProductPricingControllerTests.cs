using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ProductPricingControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
        UpdateProductPricingController sut = UpdateProductPricingController();

        ActionResult<UpdateProductPricingResponse> actionResult = await sut.UpdateProductPricing(id, request);

        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateProductPricing_UpdatesPricing_WhenProductExists()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        UpdateProductPricingRequest request = new()
        {
            BasePrice = "99.99",
            DiscountPercentage = "10",
            TaxRate = "5",
            FieldMask = ["pricing.baseprice", "pricing.discountpercentage", "pricing.taxrate"]
        };
        Mock
            .Get(MockGetProductHandler)
            .Setup(svc => svc.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = UpdateProductPricingController();

        ActionResult<UpdateProductPricingResponse> actionResult = await sut.UpdateProductPricing(product.Id, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        contentResult.ShouldNotBeNull();
        UpdateProductPricingResponse? response = contentResult.Value as UpdateProductPricingResponse;
        response.ShouldBeEquivalentTo(Mapper.Map<UpdateProductPricingResponse>(product));
    }
}
