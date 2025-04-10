// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class MapListProductsResponseHandlerTests : MapListProductsResponseHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsPetFoodResponses_WhenCategoryIsPetFood()
    {
        var productView = new ProductViewTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();
        var pagedProducts = new PagedProducts([productView], null, 10);
        var expectedProductResponse = Mapper.Map<GetPetFoodResponse>(productView);
        var query = new MapListProductsResponseQuery { PagedProducts = pagedProducts };
        IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> sut = GetListProductsResponseHandler();

        ListProductsResponse result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Results.Should().NotBeNull();
        result.Results.Count().Should().Be(1);
        result.Results.First().Should().BeOfType<GetPetFoodResponse>();
        result.Results.First().Should().BeEquivalentTo(expectedProductResponse);
    }

    [Fact]
    public async Task Handle_ReturnsGroomingResponses_WhenCategoryIsGroomingAndHygiene()
    {
        var productView = new ProductViewTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var pagedProducts = new PagedProducts([productView], null, 10);
        var expectedProductResponse = Mapper.Map<GetGroomingAndHygieneResponse>(productView);
        var query = new MapListProductsResponseQuery { PagedProducts = pagedProducts };
        IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> sut = GetListProductsResponseHandler();

        ListProductsResponse result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Results.Should().NotBeNull();
        result.Results.Count().Should().Be(1);
        result.Results.First().Should().BeOfType<GetGroomingAndHygieneResponse>();
        result.Results.First().Should().BeEquivalentTo(expectedProductResponse);
    }

    [Fact]
    public async Task Handle_ReturnsBaseResponses_WhenCategoryIsNotSpecialized()
    {
        var productView = new ProductViewTestDataBuilder()
            .WithCategory(Category.Toys)
            .Build();
        var pagedProducts = new PagedProducts([productView], null, 10);
        var expectedProductResponse = Mapper.Map<GetProductResponse>(productView);
        var query = new MapListProductsResponseQuery { PagedProducts = pagedProducts };
        IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> sut = GetListProductsResponseHandler();

        ListProductsResponse result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Results.Should().NotBeNull();
        result.Results.Count().Should().Be(1);
        result.Results.First().Should().BeOfType<GetProductResponse>();
        result.Results.First().Should().BeEquivalentTo(expectedProductResponse);
    }

    [Fact]
    public async Task Handle_ReturnsMultipleProductResponses_WhenMultipleProductsExist()
    {
        var petFoodProduct = new ProductViewTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();
        var groomingProduct = new ProductViewTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var bedProduct = new ProductViewTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var pagedProducts = new PagedProducts([petFoodProduct, groomingProduct, bedProduct], "4", 10);
        var query = new MapListProductsResponseQuery { PagedProducts = pagedProducts };
        IQueryHandler<MapListProductsResponseQuery, ListProductsResponse> sut = GetListProductsResponseHandler();

        ListProductsResponse result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Results.Should().NotBeNull();
        result.Results.Count().Should().Be(3);
        result.Results.ElementAt(0).Should().BeOfType<GetPetFoodResponse>();
        result.Results.ElementAt(1).Should().BeOfType<GetGroomingAndHygieneResponse>();
        result.Results.ElementAt(2).Should().BeOfType<GetProductResponse>();
    }
}
