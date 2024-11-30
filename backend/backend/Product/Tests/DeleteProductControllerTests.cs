using backend.Product.ProductControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class DeleteProductControllerTests
{
    private readonly DeleteProductController _controller;
    private readonly ProductRepositoryFake _productRepository = [];

    public DeleteProductControllerTests() => _controller = new DeleteProductController(_productRepository);

    [Fact]
    public async Task DeleteProduct_Should_ReturnNoContent_When_ProductExists()
    {
        DomainModels.Product product = new ProductTestDataBuilder().Build();
        _productRepository.Add(product);

        ActionResult result = await _controller.DeleteProduct(product.Id, new DeleteProductRequest());

        result.ShouldBeOfType<NoContentResult>();
        DomainModels.Product? deletedProduct = await _productRepository.GetProductAsync(product.Id);
        deletedProduct.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteProduct_Should_ReturnNotFound_When_ProductDoesNotExist()
    {
        ActionResult result = await _controller.DeleteProduct(999, new DeleteProductRequest());

        result.ShouldBeOfType<NotFoundResult>();
    }
}
