// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
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
        Repository.IsDirty = false;
        var sut = ProductViewApplicationService();

        var result = await sut.GetProductView(productView.Id);

        result.ShouldNotBeNull();
        result.ShouldBe(productView);
    }

    [Fact]
    public async Task GetProductView_ShouldReturnNull_WhenProductDoesNotExist()
    {
        long productId = Fixture.Create<long>();
        var sut = ProductViewApplicationService();

        var result = await sut.GetProductView(productId);

        result.ShouldBeNull();
    }
    //
    // [Fact]
    // public async Task ListProductsAsync_ShouldReturnProductsAndNextPageToken()
    // {
    //     // Arrange
    //     var request = new ListProductsRequest { PageToken = "page1", Filter = "filter", MaxPageSize = 10 };
    //
    //     var expectedProducts = new List<ProductView>
    //     {
    //         new ProductView { Id = 1, Name = "Product 1" }, new ProductView { Id = 2, Name = "Product 2" }
    //     };
    //
    //     var nextPageToken = "page2";
    //
    //     _repositoryMock
    //         .Setup(repo => repo.ListProductsAsync(request.PageToken, request.Filter, request.MaxPageSize))
    //         .ReturnsAsync((expectedProducts, nextPageToken));
    //
    //     // Act
    //     var (products, nextPage) = await _service.ListProductsAsync(request);
    //
    //     // Assert
    //     Assert.NotNull(products);
    //     Assert.Equal(2, products.Count);
    //     Assert.Equal("Product 1", products[0].Name);
    //     Assert.Equal("Product 2", products[1].Name);
    //     Assert.Equal(nextPageToken, nextPage);
    //     _repositoryMock.Verify(repo => repo.ListProductsAsync(request.PageToken, request.Filter, request.MaxPageSize),
    //         Times.Once);
    // }
    //
    // [Fact]
    // public async Task ListProductsAsync_ShouldReturnEmptyListAndNullNextPageToken_WhenNoProductsExist()
    // {
    //     // Arrange
    //     var request = new ListProductsRequest { PageToken = "page1", Filter = "filter", MaxPageSize = 10 };
    //
    //     _repositoryMock
    //         .Setup(repo => repo.ListProductsAsync(request.PageToken, request.Filter, request.MaxPageSize))
    //         .ReturnsAsync((new List<ProductView>(), (string?)null));
    //
    //     // Act
    //     var (products, nextPage) = await _service.ListProductsAsync(request);
    //
    //     // Assert
    //     Assert.NotNull(products);
    //     Assert.Empty(products);
    //     Assert.Null(nextPage);
    //     _repositoryMock.Verify(repo => repo.ListProductsAsync(request.PageToken, request.Filter, request.MaxPageSize),
    //         Times.Once);
    // }
    //
    // [Fact]
    // public async Task ListProductsAsync_ShouldReturnCorrectPageToken_WhenProvided()
    // {
    //     // Arrange
    //     var request = new ListProductsRequest { PageToken = "page1", Filter = "filter", MaxPageSize = 2 };
    //
    //     var expectedProducts = new List<ProductView>
    //     {
    //         new ProductView { Id = 1, Name = "Product 1" }, new ProductView { Id = 2, Name = "Product 2" }
    //     };
    //
    //     var nextPageToken = "page2";
    //
    //     _repositoryMock
    //         .Setup(repo => repo.ListProductsAsync(request.PageToken, request.Filter, request.MaxPageSize))
    //         .ReturnsAsync((expectedProducts, nextPageToken));
    //
    //     // Act
    //     var (products, nextPage) = await _service.ListProductsAsync(request);
    //
    //     // Assert
    //     Assert.NotNull(products);
    //     Assert.Equal(2, products.Count);
    //     Assert.Equal("Product 1", products[0].Name);
    //     Assert.Equal("Product 2", products[1].Name);
    //     Assert.Equal(nextPageToken, nextPage);
    //     _repositoryMock.Verify(repo => repo.ListProductsAsync(request.PageToken, request.Filter, request.MaxPageSize),
    //         Times.Once);
    // }
}
