using backend.Product.ProductControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class CreateProductControllerTests
{
    private readonly CreateProductController _controller;
    private readonly ProductRepositoryFake _productRepository = [];
    private readonly CreateProductExtensions _extensions;

    public CreateProductControllerTests()
    {
        _extensions = new CreateProductExtensions(new TypeParser());
        _controller = new CreateProductController(_productRepository, _extensions);
    }

    [Fact]
    public async Task CreateProduct_Should_AddProduct_And_ReturnCreatedAtActionResult()
    {
        var product = new ProductTestDataBuilder().AsToys().WithId(5).Build();
        var request = _extensions.ToCreateProductRequest(product);
        var expectedResponse = _extensions.ToCreateProductResponse(product);
        _productRepository.Add(product);

        var result = await _controller.CreateProduct(request);

        var createdAtActionResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
        var actualResponse = createdAtActionResult.Value.ShouldBeOfType<CreateProductResponse>();
        actualResponse.ShouldBeEquivalentTo(expectedResponse);
    }
}