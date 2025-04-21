// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class GetProductHandlerTests : GetProductHandlerTestBase
{
    [Fact]
    public async Task GetProductAsync_ReturnsProduct_WhenProductExists()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        Repository.Add(expectedProduct);
        var sut = GetProductHandler();

        DomainModels.Product? result = await sut.Handle(new GetProductQuery { Id = expectedProduct.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task GetProductAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        var sut = GetProductHandler();

        DomainModels.Product? result = await sut.Handle(new GetProductQuery { Id = expectedProduct.Id });

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProductAsync_ReturnsPetFoodProduct_WhenCategoryIsPetFood()
    {
        var petFoodProduct = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();
        Repository.Add(petFoodProduct);
        var sut = GetProductHandler();

        DomainModels.Product? result = await sut.Handle(new GetProductQuery { Id = petFoodProduct.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(petFoodProduct);
    }

    [Fact]
    public async Task GetProductAsync_ReturnsGroomingProduct_WhenCategoryIsGroomingAndHygiene()
    {
        var groomingProduct = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        Repository.Add(groomingProduct);
        var sut = GetProductHandler();

        DomainModels.Product? result = await sut.Handle(new GetProductQuery { Id = groomingProduct.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(groomingProduct);
    }
}
