// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class MapCreateProductResponseHandlerTests : MapCreateProductResponseHandlerTestBase
{
    [Fact]
    public void Handle_ReturnsPetFoodResponse_WhenCategoryIsPetFood()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();

        var expectedResponse = Mapper.Map<CreatePetFoodResponse>(product);
        var query = new MapCreateProductResponseQuery { Product = product };
        var sut = GetCreateProductResponseHandler();

        CreateProductResponse result =  sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<CreatePetFoodResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void Handle_ReturnsGroomingResponse_WhenCategoryIsGroomingAndHygiene()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var expectedResponse = Mapper.Map<CreateGroomingAndHygieneResponse>(product);
        var query = new MapCreateProductResponseQuery { Product = product };
        var sut = GetCreateProductResponseHandler();

        CreateProductResponse result =  sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<CreateGroomingAndHygieneResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void Handle_ReturnsBaseResponse_WhenCategoryIsNotSpecialized()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var expectedResponse = Mapper.Map<CreateProductResponse>(product);
        var query = new MapCreateProductResponseQuery { Product = product };
        var sut = GetCreateProductResponseHandler();

        CreateProductResponse result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<CreateProductResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }
}
