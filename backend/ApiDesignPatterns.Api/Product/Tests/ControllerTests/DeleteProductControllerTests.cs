using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.Product;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class DeleteProductControllerTests : DeleteProductControllerTestBase
{
    [Fact]
    public async Task DeleteProduct_ProductExists_ReturnsNoContent()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        Mock
            .Get(MockGetProductHandler)
            .Setup(svc => svc.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        DeleteProductController sut = DeleteProductController();

        ActionResult result = await sut.DeleteProduct(product.Id, new DeleteProductRequest());

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteProduct_ProductDoesNotExist_ReturnsNotFound()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        Mock
            .Get(MockGetProductHandler)
            .Setup(svc => svc.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync((DomainModels.Product?)null);
        var sut = DeleteProductController();

        ActionResult result = await sut.DeleteProduct(product.Id, new DeleteProductRequest());

        result.Should().BeOfType<NotFoundResult>();
    }
}
