using backend.Product.ApplicationLayer.Commands.DeleteProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
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

        result.ShouldBeOfType<NoContentResult>();
        Mock
            .Get(MockDeleteProductHandler)
            .Verify(svc => svc.Handle(new DeleteProductCommand { Id = product.Id }), Times.Once);
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

        result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(MockDeleteProductHandler)
            .Verify(svc => svc.Handle(new DeleteProductCommand { Id = product.Id }), Times.Never);
    }
}
