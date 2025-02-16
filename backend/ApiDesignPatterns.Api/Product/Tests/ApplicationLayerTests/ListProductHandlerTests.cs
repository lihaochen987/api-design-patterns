// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.ProductControllers;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ListProductHandlerTests : ListProductHandlerTestBase
{
    [Fact]
    public async Task ListProductsAsync_ShouldReturnProductsAndNextPageToken()
    {
        var request = new ListProductsRequest { Filter = "Category == \"Toys\"", MaxPageSize = 5 };
        Repository.AddProductView(1, Category.Toys);
        Repository.AddProductView(2, Category.Toys);
        IQueryHandler<ListProductsQuery, PagedProducts> sut = ListProductsViewHandler();

        PagedProducts? result =
            await sut.Handle(
                new ListProductsQuery
                {
                    Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
                });

        result!.Products.ShouldNotBeEmpty();
        result.Products.Count.ShouldBe(2);
        result.NextPageToken.ShouldBe(null);
    }

    [Fact]
    public async Task ListProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        var request = new ListProductsRequest();
        IQueryHandler<ListProductsQuery, PagedProducts> sut = ListProductsViewHandler();

        PagedProducts? result = await sut.Handle(new ListProductsQuery
        {
            Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
        });

        result!.Products.ShouldBeEmpty();
        result.NextPageToken.ShouldBeNull();
    }

    [Fact]
    public async Task ListProductsAsync_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        var request = new ListProductsRequest
        {
            PageToken = "1", Filter = "InvalidFilter == \"SomeValue\"", MaxPageSize = 5
        };
        IQueryHandler<ListProductsQuery, PagedProducts> sut = ListProductsViewHandler();

        await Should.ThrowAsync<ArgumentException>(() => sut.Handle(new ListProductsQuery
        {
            Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
        }));
    }
}
