using backend.Product.ApplicationLayer.DeleteProduct;
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
            .Get(MockProductQueryService)
            .Setup(svc => svc.GetProductAsync(product.Id))
            .ReturnsAsync(product);
        DeleteProductController sut = DeleteProductController();

        ActionResult result = await sut.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NoContentResult>();
        Mock
            .Get(MockDeleteProductHandler)
            .Verify(svc => svc.Handle(new DeleteProduct { Product = product }), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_ProductDoesNotExist_ReturnsNotFound()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        Mock
            .Get(MockProductQueryService)
            .Setup(svc => svc.GetProductAsync(product.Id))
            .ReturnsAsync((DomainModels.Product?)null);
        var sut = DeleteProductController();

        ActionResult result = await sut.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(MockDeleteProductHandler)
            .Verify(svc => svc.Handle(new DeleteProduct { Product = product }), Times.Never);
    }
}
