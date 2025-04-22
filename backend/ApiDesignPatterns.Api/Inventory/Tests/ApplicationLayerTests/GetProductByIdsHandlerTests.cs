// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetProductsByIds;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetProductsByIdsHandlerTests : GetProductsByIdsHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsProducts_WhenProductsExist()
    {
        var productOne = new ProductViewTestDataBuilder().Build();
        var productTwo = new ProductViewTestDataBuilder().Build();
        var expectedProducts = new List<ProductView> { productOne, productTwo };
        var productIds = expectedProducts.Select(p => p.Id).ToList();
        Repository.Add(productOne);
        Repository.Add(productTwo);
        var sut = GetProductsByIdsHandler();
        var query = new GetProductsByIdsQuery { ProductIds = productIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoProductsExist()
    {
        var productIds = new List<long> { Fixture.Create<long>(), Fixture.Create<long>() };
        var sut = GetProductsByIdsHandler();
        var query = new GetProductsByIdsQuery { ProductIds = productIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsOnlyFoundProducts_WhenSomeExist()
    {
        var existingProduct = new ProductViewTestDataBuilder().Build();
        var existingProducts = new List<ProductView> { existingProduct };
        var productIds = new List<long> { existingProduct.Id, Fixture.Create<long>() };
        Repository.Add(existingProduct);
        var sut = GetProductsByIdsHandler();
        var query = new GetProductsByIdsQuery { ProductIds = productIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(existingProducts);
    }
}
