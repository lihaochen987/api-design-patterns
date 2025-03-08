// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class CreateProductControllerTests : CreateProductControllerTestBase
{
    [Fact]
    public async Task CreateProduct_ReturnsCreatedResponse_WhenProductCreatedSuccessfully()
    {
        var product = new ProductTestDataBuilder().Build();
        var request = Mapper.Map<CreateProductRequest>(product);
        var expectedResponse = Mapper.Map<CreateProductResponse>(product);
        Mock
            .Get(CreateProductResponse)
            .Setup(x => x.Handle(It.IsAny<MapCreateProductResponseQuery>()))
            .ReturnsAsync(expectedResponse);
        var sut = GetCreateProductController();

        var result = await sut.CreateProduct(request);

        result.ShouldNotBeNull();
        var createdResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
        createdResult.ActionName.ShouldBe("GetProduct");
        createdResult.ControllerName.ShouldBe("GetProduct");
        createdResult.Value.ShouldBeEquivalentTo(expectedResponse);
        Mock
            .Get(CreateProduct)
            .Verify(x => x.Handle(It.IsAny<CreateProductCommand>()), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_ReturnsPetFoodResponse_WhenProductIsPetFood()
    {
        var product = new ProductTestDataBuilder().WithCategory(Category.PetFood).Build();
        var request = Mapper.Map<CreateProductRequest>(product);
        var expectedResponse = Mapper.Map<CreatePetFoodResponse>(product);
        Mock
            .Get(CreateProductResponse)
            .Setup(x => x.Handle(It.IsAny<MapCreateProductResponseQuery>()))
            .ReturnsAsync(expectedResponse);
        CreateProductController sut = GetCreateProductController();

        await sut.CreateProduct(request);

        Mock
            .Get(CreateProduct)
            .Verify(x => x.Handle(It.IsAny<CreateProductCommand>()), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_ReturnsGroomingResponse_WhenProductIsGroomingAndHygiene()
    {
        var product = new ProductTestDataBuilder().WithCategory(Category.GroomingAndHygiene).Build();
        var request = Mapper.Map<CreateProductRequest>(product);
        var expectedResponse = Mapper.Map<CreateGroomingAndHygieneResponse>(product);
        Mock
            .Get(CreateProductResponse)
            .Setup(x => x.Handle(It.IsAny<MapCreateProductResponseQuery>()))
            .ReturnsAsync(expectedResponse);
        CreateProductController sut = GetCreateProductController();

        await sut.CreateProduct(request);

        Mock
            .Get(CreateProduct)
            .Verify(x => x.Handle(It.IsAny<CreateProductCommand>()), Times.Once);
    }
}
