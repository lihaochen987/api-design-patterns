using backend.Product.ApplicationLayer.UpdateProduct;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class UpdateProductControllerTests : UpdateProductControllerTestBase
{
    [Fact]
    public async Task UpdateProduct_WithEmptyFieldMask_ShouldUpdateNoFields()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        UpdateProductRequest request = new()
        {
            Name = "Updated Name", Pricing = new ProductPricingRequest { BasePrice = "1.99" }, Category = "Toys"
        };
        Mock
            .Get(MockProductQueryService)
            .Setup(service => service.GetProductAsync(product.Id))
            .ReturnsAsync(product);
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(product.Id, request);

        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = actionResult.Result as OkObjectResult;
        UpdateProductResponse? response = contentResult!.Value as UpdateProductResponse;
        response.ShouldBeEquivalentTo(Mapper.Map<UpdateProductResponse>(product));
        Mock
            .Get(MockUpdateProductService)
            .Verify(
                svc => svc.Execute(new UpdateProduct { Request = request, Product = product }));
    }

    [Fact]
    public async Task UpdateProduct_NonExistentProduct_ShouldReturnNotFound()
    {
        UpdateProductRequest request = new() { Name = "Non-Existent Product", FieldMask = ["name"] };
        const int nonExistentId = 999;
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(nonExistentId, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<NotFoundResult>();
    }
}
