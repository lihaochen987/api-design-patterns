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
    private readonly CreateProductExtensions _extensions;
    private readonly ProductRepositoryFake _productRepository = [];

    public CreateProductControllerTests()
    {
        _extensions = new CreateProductExtensions(new TypeParser());
        _controller = new CreateProductController(_productRepository, _extensions);
    }

    // [Fact]
    // public async Task CreateProduct_Should_AddProduct_And_ReturnCreatedAtActionResult()
    // {
    //     DomainModels.Product product = new ProductTestDataBuilder().AsToys().WithId(5).Build();
    //     CreateProductRequest request = new CreateProductRequest
    //     {
    //
    //     }
    //     CreateProductResponse expectedResponse = _extensions.ToCreateProductResponse(product);
    //     _productRepository.Add(product);
    //
    //     ActionResult<CreateProductResponse> result = await _controller.CreateProduct(request);
    //
    //     CreatedAtActionResult createdAtActionResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
    //     CreateProductResponse actualResponse = createdAtActionResult.Value.ShouldBeOfType<CreateProductResponse>();
    //     actualResponse.ShouldBeEquivalentTo(expectedResponse);
    // }
}
