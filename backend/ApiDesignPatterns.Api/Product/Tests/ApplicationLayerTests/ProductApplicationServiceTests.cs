// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Tests.TestHelpers.Builders;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ProductApplicationServiceTests : ProductApplicationServiceTestBase
{
    [Fact]
    public async Task GetProductAsync_ReturnsProduct_WhenProductExists()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        Repository.Add(expectedProduct);
        var sut = ProductApplicationService();

        DomainModels.Product? result = await sut.GetProductAsync(expectedProduct.Id);

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedProduct);
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
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 1);
    }

    [Fact]
    public async Task CreateProductAsync_PersistsWhenCalledTwice()
    {
        DomainModels.Product firstProductToCreate = new ProductTestDataBuilder().Build();
        DomainModels.Product secondProductToCreate = new ProductTestDataBuilder().Build();
        var sut = ProductApplicationService();

        await sut.CreateProductAsync(firstProductToCreate);
        await sut.CreateProductAsync(secondProductToCreate);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("CreateProductAsync", 2);
    }

    [Fact]
    public async Task DeleteProductAsync_CallsRepositoryWithCorrectProduct()
    {
        DomainModels.Product productToDelete = new ProductTestDataBuilder().Build();
        var sut = ProductApplicationService();

        await sut.DeleteProductAsync(productToDelete);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("DeleteProductAsync", 1);
    }

    [Fact]
    public async Task UpdateProductAsync_ValidProduct_ShouldCallRepositoryUpdateProductAsync()
    {
        var existingProduct = new ProductTestDataBuilder().WithId(1).Build();
        Repository.Add(existingProduct);
        Repository.IsDirty = false;
        var updatedProduct = new ProductTestDataBuilder().WithId(1).Build();
        var sut = ProductApplicationService();

        await sut.UpdateProductAsync(updatedProduct);

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.Count.ShouldBe(1);
        Repository.CallCount.ShouldContainKeyAndValue("UpdateProductAsync", 1);
    }
}
