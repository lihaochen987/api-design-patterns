// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class MapCreateProductResponseHandlerTests : MapCreateProductResponseHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsPetFoodResponse_WhenCategoryIsPetFood()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();

        var expectedResponse = Mapper.Map<CreatePetFoodResponse>(product);
        var query = new MapCreateProductResponseQuery { Product = product };
        IQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> sut = GetCreateProductResponseHandler();

        CreateProductResponse? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<CreatePetFoodResponse>();
        result.ShouldBeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_ReturnsGroomingResponse_WhenCategoryIsGroomingAndHygiene()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var expectedResponse = Mapper.Map<CreateGroomingAndHygieneResponse>(product);
        var query = new MapCreateProductResponseQuery { Product = product };
        IQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> sut = GetCreateProductResponseHandler();

        CreateProductResponse? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<CreateGroomingAndHygieneResponse>();
        result.ShouldBeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_ReturnsBaseResponse_WhenCategoryIsNotSpecialized()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var expectedResponse = Mapper.Map<CreateProductResponse>(product);
        var query = new MapCreateProductResponseQuery { Product = product };
        IQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> sut = GetCreateProductResponseHandler();

        CreateProductResponse? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<CreateProductResponse>();
        result.ShouldBeEquivalentTo(expectedResponse);
    }
}
