using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.ProductPricing;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

        actionResult.Result.Should().BeOfType<NotFoundResult>();
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

        actionResult.Result.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult? contentResult = (OkObjectResult)actionResult.Result;
        contentResult.Should().NotBeNull();
        UpdateProductPricingResponse response = (UpdateProductPricingResponse)contentResult.Value!;
        response.Should().BeEquivalentTo(Mapper.Map<UpdateProductPricingResponse>(product));
    }
}
