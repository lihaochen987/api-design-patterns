// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class CreateProductControllerTests : CreateProductControllerTestBase
{
    [Fact]
    public async Task CreateProduct_ReturnsCreatedResponse_WhenProductCreatedSuccessfully()
    {
        var product = new ProductTestDataBuilder().WithCategory(Category.Beds).Build();
        var request = Mapper.Map<CreateProductRequest>(product);
        var expectedResponse = Mapper.Map<CreateProductResponse>(product);
        var sut = GetCreateProductController();

        var result = await sut.CreateProduct(request);

        result.Should().NotBe(null);
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = (CreatedAtActionResult)result.Result;
        createdResult.ActionName.Should().BeEquivalentTo("GetProduct");
        createdResult.ControllerName.Should().BeEquivalentTo("GetProduct");
        createdResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.Excluding(x => x.Id));
        Mock
            .Get(CreateProduct)
            .Verify(x => x.Handle(It.IsAny<CreateProductCommand>()), Times.Once);
    }
}
