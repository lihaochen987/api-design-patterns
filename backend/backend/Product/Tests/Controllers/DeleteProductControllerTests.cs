using backend.Product.ApplicationLayer;
using backend.Product.ProductControllers;
using backend.Product.Tests.Helpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.Controllers;

public class DeleteProductControllerTests
{
    private readonly DeleteProductController _controller;
    private readonly IProductApplicationService _mockApplicationService = Mock.Of<IProductApplicationService>();

    public DeleteProductControllerTests() => _controller = new DeleteProductController(_mockApplicationService);

    [Fact]
    public async Task DeleteProduct_ProductExists_ReturnsNoContent()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        Mock
            .Get(_mockApplicationService)
            .Setup(svc => svc.GetProductAsync(product.Id))
            .ReturnsAsync(product);
        Mock
            .Get(_mockApplicationService)
            .Setup(svc => svc.DeleteProductAsync(product))
            .Returns(Task.CompletedTask);

        ActionResult result = await _controller.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NoContentResult>();
        Mock
            .Get(_mockApplicationService)
            .Verify(svc => svc.GetProductAsync(product.Id), Times.Once);
        Mock
            .Get(_mockApplicationService)
            .Verify(svc => svc.DeleteProductAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_ProductDoesNotExist_ReturnsNotFound()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        Mock
            .Get(_mockApplicationService)
            .Setup(svc => svc.GetProductAsync(product.Id))
            .ReturnsAsync((DomainModels.Product?)null);

        ActionResult result = await _controller.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(_mockApplicationService)
            .Verify(svc => svc.GetProductAsync(product.Id), Times.Once);
        Mock
            .Get(_mockApplicationService)
            .Verify(svc => svc.DeleteProductAsync(product), Times.Never);
    }
}
