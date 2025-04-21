// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class MapReplaceProductResponseHandlerTests : MapReplaceProductResponseHandlerTestBase
{
    [Fact]
    public void Handle_ReturnsPetFoodResponse_WhenCategoryIsPetFood()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();

        var expectedResponse = Mapper.Map<ReplacePetFoodResponse>(product);
        var query = new MapReplaceProductResponseQuery { Product = product };
        var sut = GetReplaceProductResponseHandler();

        ReplaceProductResponse result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<ReplacePetFoodResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void Handle_ReturnsGroomingResponse_WhenCategoryIsGroomingAndHygiene()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var expectedResponse = Mapper.Map<ReplaceGroomingAndHygieneResponse>(product);
        var query = new MapReplaceProductResponseQuery { Product = product };
        var sut = GetReplaceProductResponseHandler();

        ReplaceProductResponse result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<ReplaceGroomingAndHygieneResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public void Handle_ReturnsBaseResponse_WhenCategoryIsNotSpecialized()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var expectedResponse = Mapper.Map<ReplaceProductResponse>(product);
        var query = new MapReplaceProductResponseQuery { Product = product };
        var sut = GetReplaceProductResponseHandler();

        ReplaceProductResponse result = sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeOfType<ReplaceProductResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }
}
