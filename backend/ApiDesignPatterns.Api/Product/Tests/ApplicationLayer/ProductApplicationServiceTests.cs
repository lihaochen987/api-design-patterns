// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Tests.Helpers.Builders;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayer;

public class ProductApplicationServiceTests : ProductApplicationServiceTestBase
{
    [Fact]
    public async Task GetProductAsync_ReturnsProduct_WhenProductExists()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        Repository.Add(expectedProduct);
        Repository.IsDirty = false;
        var sut = ProductApplicationService();

        DomainModels.Product? result = await sut.GetProductAsync(expectedProduct.Id);

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedProduct);
        Repository.IsDirty.ShouldBeFalse();
    }

    [Fact]
    public async Task GetProductAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        var sut = ProductApplicationService();

        DomainModels.Product? result = await sut.GetProductAsync(expectedProduct.Id);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task CreateProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToCreate = new ProductTestDataBuilder().Build();
        var sut = ProductApplicationService();

        await sut.CreateProductAsync(productToCreate);

        Repository.IsDirty.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToDelete = new ProductTestDataBuilder().Build();
        var sut = ProductApplicationService();

        await sut.DeleteProductAsync(productToDelete);

        Repository.IsDirty.ShouldBeTrue();
    }
}
