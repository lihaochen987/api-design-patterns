using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            Name = "Updated Name", Pricing = new ProductPricingRequest { BasePrice = 1.99m }, Category = "Toys"
        };
        Mock
            .Get(MockGetProductHandler)
            .Setup(svc => svc.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(product.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        OkObjectResult? contentResult = (OkObjectResult)actionResult.Result;
        UpdateProductResponse response = (UpdateProductResponse)contentResult.Value!;
        response.Should().BeEquivalentTo(Mapper.Map<UpdateProductResponse>(product));
        Mock
            .Get(MockUpdateProductHandler)
            .Verify(
                svc => svc.Handle(new UpdateProductCommand { Request = request, Product = product }));
    }

    [Fact]
    public async Task UpdateProduct_NonExistentProduct_ShouldReturnNotFound()
    {
        UpdateProductRequest request = new() { Name = "Non-Existent Product", FieldMask = ["name"] };
        const int nonExistentId = 999;
        var sut = UpdateProductController();

        ActionResult<UpdateProductResponse> actionResult = await sut.UpdateProduct(nonExistentId, request);

        actionResult.Result.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }
}
