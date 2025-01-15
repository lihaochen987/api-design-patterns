// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ProductViewApplicationServiceTests : ProductViewApplicationServiceTestBase
{
    [Fact]
    public async Task GetProductView_ShouldReturnProduct_WhenProductExists()
    {
        var productView = new ProductViewTestDataBuilder().WithName("Sample Product").Build();
        Repository.Add(productView);
        ProductViewQueryApplicationService sut = ProductViewApplicationService();

        ProductView? result = await sut.GetProductView(productView.Id);

        result.ShouldNotBeNull();
        result.ShouldBe(productView);
    }

    [Fact]
    public async Task GetProductView_ShouldReturnNull_WhenProductDoesNotExist()
    {
        long productId = Fixture.Create<long>();
        ProductViewQueryApplicationService sut = ProductViewApplicationService();

        ProductView? result = await sut.GetProductView(productId);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task ListProductsAsync_ShouldReturnProductsAndNextPageToken()
    {
        var request = new ListProductsRequest { Filter = "Category == \"Toys\"", MaxPageSize = 5 };
        ProductView productOne = new ProductViewTestDataBuilder().WithId(1).WithCategory(Category.Toys).Build();
        ProductView productTwo = new ProductViewTestDataBuilder().WithId(2).WithCategory(Category.Toys).Build();
        Repository.Add(productOne);
        Repository.Add(productTwo);
        ProductViewQueryApplicationService sut = ProductViewApplicationService();

        (List<ProductView>, string?) result = await sut.ListProductsAsync(request);

        result.Item1.ShouldNotBeEmpty();
        result.Item1.Count.ShouldBe(2);
        result.Item2.ShouldBe(null);
    }

    [Fact]
    public async Task ListProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        var request = new ListProductsRequest();
        ProductViewQueryApplicationService sut = ProductViewApplicationService();

        (List<ProductView>, string?) result = await sut.ListProductsAsync(request);

        result.Item1.ShouldBeEmpty();
        result.Item2.ShouldBeNull();
    }

    [Fact]
    public async Task ListProductsAsync_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        var request = new ListProductsRequest
        {
            PageToken = "1", Filter = "InvalidFilter == \"SomeValue\"", MaxPageSize = 5
        };
        ProductViewQueryApplicationService sut = ProductViewApplicationService();

        await Should.ThrowAsync<ArgumentException>(() => sut.ListProductsAsync(request));
    }
}
