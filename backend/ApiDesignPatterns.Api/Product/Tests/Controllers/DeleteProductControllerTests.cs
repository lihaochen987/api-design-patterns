using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using backend.Product.Tests.Helpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.Controllers;

public class DeleteProductControllerTests : DeleteProductControllerTestBase
{
    [Fact]
    public async Task DeleteProduct_ProductExists_ReturnsNoContent()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        Mock
            .Get(MockApplicationService)
            .Setup(svc => svc.GetProductAsync(product.Id))
            .ReturnsAsync(product);
        var sut = DeleteProductController();

        ActionResult result = await sut.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NoContentResult>();
        Mock
            .Get(MockApplicationService)
            .Verify(svc => svc.DeleteProductAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_ProductDoesNotExist_ReturnsNotFound()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        Mock
            .Get(MockApplicationService)
            .Setup(svc => svc.GetProductAsync(product.Id))
            .ReturnsAsync((DomainModels.Product?)null);
        var sut = DeleteProductController();

        ActionResult result = await sut.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(MockApplicationService)
            .Verify(svc => svc.DeleteProductAsync(product), Times.Never);
    }
}
