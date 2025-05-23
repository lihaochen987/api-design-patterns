// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Product.Tests.ControllerTests;

public class ReplaceProductControllerTests : ReplaceProductControllerTestBase
{
    [Fact]
    public async Task ReplaceProduct_ReturnsOkResponse_WhenProductReplacedSuccessfully()
    {
        var product = new ProductTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceProductRequest>(product);
        var expectedResponse = Mapper.Map<ReplaceProductResponse>(product);
        Mock
            .Get(GetProduct)
            .Setup(x => x.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        ReplaceProductController sut = GetReplaceProductController();

        var result = await sut.ReplaceProduct(product.Id, request);

        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task ReplaceProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        const long nonExistentId = 999;
        var product = new ProductTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceProductRequest>(product);
        Mock
            .Get(GetProduct)
            .Setup(x => x.Handle(It.Is<GetProductQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.Product?)null);
        ReplaceProductController sut = GetReplaceProductController();

        var result = await sut.ReplaceProduct(nonExistentId, request);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task ReplaceProduct_ReturnsPetFoodResponse_WhenProductIsPetFood()
    {
        var product = new ProductTestDataBuilder().WithCategory(Category.PetFood).Build();
        var request = Mapper.Map<ReplaceProductRequest>(product);
        Mock
            .Get(GetProduct)
            .Setup(x => x.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = GetReplaceProductController();

        var result = await sut.ReplaceProduct(product.Id, request);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReplaceProduct_ReturnsGroomingResponse_WhenProductIsGroomingAndHygiene()
    {
        var product = new ProductTestDataBuilder().WithCategory(Category.GroomingAndHygiene).Build();
        var request = Mapper.Map<ReplaceProductRequest>(product);
        Mock
            .Get(GetProduct)
            .Setup(x => x.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product);
        var sut = GetReplaceProductController();

        var result = await sut.ReplaceProduct(product.Id, request);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReplaceProduct_ReturnsNotFound_WhenProductDisappearsAfterReplacement()
    {
        var product = new ProductTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceProductRequest>(product);
        Mock
            .Get(GetProduct)
            .SetupSequence(x => x.Handle(It.Is<GetProductQuery>(q => q.Id == product.Id)))
            .ReturnsAsync(product)
            .ReturnsAsync((DomainModels.Product?)null);
        ReplaceProductController sut = GetReplaceProductController();

        var result = await sut.ReplaceProduct(product.Id, request);

        result.Result.Should().BeOfType<NotFoundResult>();
    }
}
