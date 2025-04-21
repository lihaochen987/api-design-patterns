// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Shared.QueryHandler;
using FluentAssertions;
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
        var sut = ListProductsViewHandler();

        PagedProducts result =
            await sut.Handle(
                new ListProductsQuery
                {
                    Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
                });

        result.Products.Should().NotBeEmpty();
        result.Products.Count.Should().Be(2);
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        var request = new ListProductsRequest();
        var sut = ListProductsViewHandler();

        PagedProducts result = await sut.Handle(new ListProductsQuery
        {
            Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
        });

        result.Products.Should().BeEmpty();
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProductsAsync_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        var request = new ListProductsRequest
        {
            PageToken = "1", Filter = "InvalidFilter == \"SomeValue\"", MaxPageSize = 5
        };
        var sut = ListProductsViewHandler();

        Func<Task> act = async () => await sut.Handle(new ListProductsQuery
        {
            Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = request.PageToken
        });

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ListProductsAsync_FirstPage_ShouldReturnCorrectProductCountAndTotalCount()
    {
        var request = new ListProductsRequest { Filter = "Category == \"Toys\"", MaxPageSize = 2 };
        SetupToyProducts(5);
        var sut = ListProductsViewHandler();

        PagedProducts result = await sut.Handle(
            new ListProductsQuery { Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = null });

        result.Products.Count.Should().Be(2);
        result.TotalCount.Should().Be(5);
        result.NextPageToken.Should().NotBeNull();
    }

    [Fact]
    public async Task ListProductsAsync_SecondPage_ShouldMaintainSameTotalCountAsFirstPage()
    {
        var request = new ListProductsRequest { Filter = "Category == \"Toys\"", MaxPageSize = 2 };
        SetupToyProducts(5);
        var sut = ListProductsViewHandler();
        string secondPageToken = await GetPageTokenForPage(sut, request.Filter, request.MaxPageSize, 1);

        PagedProducts result = await sut.Handle(
            new ListProductsQuery
            {
                Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = secondPageToken
            });

        result.Products.Count.Should().Be(2);
        result.TotalCount.Should().Be(5);
        result.NextPageToken.Should().NotBeNull();
    }

    [Fact]
    public async Task ListProductsAsync_LastPage_ShouldMaintainSameTotalCountAndHaveNoNextToken()
    {
        var request = new ListProductsRequest { Filter = "Category == \"Toys\"", MaxPageSize = 2 };
        SetupToyProducts(5);
        var sut = ListProductsViewHandler();
        string lastPageToken = await GetPageTokenForPage(sut, request.Filter, request.MaxPageSize, 2);

        PagedProducts result = await sut.Handle(
            new ListProductsQuery
            {
                Filter = request.Filter, MaxPageSize = request.MaxPageSize, PageToken = lastPageToken
            });

        result.Products.Count.Should().Be(1);
        result.TotalCount.Should().Be(5);
        result.NextPageToken.Should().BeNull();
    }

    private void SetupToyProducts(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            Repository.AddProductView(i, Category.Toys);
        }
    }

    private static async Task<string> GetPageTokenForPage(
        IAsyncQueryHandler<ListProductsQuery, PagedProducts> handler,
        string filter,
        int maxPageSize,
        int targetPage)
    {
        string? token = null;

        for (int i = 0; i < targetPage; i++)
        {
            PagedProducts result = await handler.Handle(
                new ListProductsQuery { Filter = filter, MaxPageSize = maxPageSize, PageToken = token });

            token = result.NextPageToken;

            if (token == null)
                break;
        }

        return token!;
    }
}
