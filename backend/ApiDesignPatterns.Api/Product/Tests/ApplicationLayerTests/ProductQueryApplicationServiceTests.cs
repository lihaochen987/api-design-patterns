// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer;
using backend.Product.Tests.TestHelpers.Builders;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ProductQueryApplicationServiceTests : ProductQueryApplicationServiceTestBase
{
    [Fact]
    public async Task GetProductAsync_ReturnsProduct_WhenProductExists()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        Repository.Add(expectedProduct);
        ProductQueryApplicationService sut = ProductQueryApplicationService();

        DomainModels.Product? result = await sut.GetProductAsync(expectedProduct.Id);

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task GetProductAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        ProductQueryApplicationService sut = ProductQueryApplicationService();

        DomainModels.Product? result = await sut.GetProductAsync(expectedProduct.Id);

        result.ShouldBeNull();
    }
}
