using backend.Product.ProductControllers;
using backend.Product.Tests.Builders;
using backend.Product.Tests.Fakes;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace backend.Product.Tests;

public class ReplaceProductControllerTests
{
    private readonly ReplaceProductController _controller;
    private readonly ReplaceProductExtensions _extensions;
    private readonly ProductRepositoryFake _productRepository = [];

    public ReplaceProductControllerTests()
    {
        _extensions = new ReplaceProductExtensions(new TypeParser());
        _controller = new ReplaceProductController(_productRepository, _extensions);
    }

    [Fact]
    public async Task ReplaceProduct_Should_ReturnOk_WithUpdatedProduct_When_ProductExists()
    {
        DomainModels.Product originalProduct = new ProductTestDataBuilder().AsToys().Build();
        _productRepository.Add(originalProduct);
        _productRepository.IsDirty = false;

        ReplaceProductRequest request = _extensions.ToReplaceProductRequest(originalProduct);

        ActionResult<ReplaceProductResponse> result = await _controller.ReplaceProduct(originalProduct.Id, request);

        result.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? response = result.Result as OkObjectResult;
        response!.Value.ShouldBeOfType<ReplaceProductResponse>();
        ReplaceProductResponse? replaceProductResponse = response.Value as ReplaceProductResponse;
        replaceProductResponse.ShouldBeEquivalentTo(_extensions.ToReplaceProductResponse(originalProduct));
        DomainModels.Product? updatedProduct = await _productRepository.GetProductAsync(originalProduct.Id);
        updatedProduct.ShouldNotBeNull();
        updatedProduct.ShouldBeEquivalentTo(originalProduct);
        _productRepository.IsDirty.ShouldBeEquivalentTo(true);
    }
}
