// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.ApplicationLayer.Queries.MapListProductsResponse;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using Shouldly;
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

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
        result.Results.Count().ShouldBe(1);
        result.Results.First().ShouldBeOfType<GetPetFoodResponse>();
        result.Results.First().ShouldBeEquivalentTo(expectedProductResponse);
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

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
        result.Results.Count().ShouldBe(1);
        result.Results.First().ShouldBeOfType<GetGroomingAndHygieneResponse>();
        result.Results.First().ShouldBeEquivalentTo(expectedProductResponse);
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

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
        result.Results.Count().ShouldBe(1);
        result.Results.First().ShouldBeOfType<GetProductResponse>();
        result.Results.First().ShouldBeEquivalentTo(expectedProductResponse);
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

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
        result.Results.Count().ShouldBe(3);
        result.Results.ElementAt(0).ShouldBeOfType<GetPetFoodResponse>();
        result.Results.ElementAt(1).ShouldBeOfType<GetGroomingAndHygieneResponse>();
        result.Results.ElementAt(2).ShouldBeOfType<GetProductResponse>();
    }
}
