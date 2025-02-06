// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.ReplaceProductResponse;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class ReplaceProductResponseHandlerTests : ReplaceProductResponseHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsPetFoodResponse_WhenCategoryIsPetFood()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();

        var expectedResponse = Mapper.Map<ReplacePetFoodResponse>(product);
        var query = new ReplaceProductResponseQuery { Product = product };
        IQueryHandler<ReplaceProductResponseQuery, ReplaceProductResponse> sut = GetReplaceProductResponseHandler();

        ReplaceProductResponse? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ReplacePetFoodResponse>();
        result.ShouldBeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_ReturnsGroomingResponse_WhenCategoryIsGroomingAndHygiene()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        var expectedResponse = Mapper.Map<ReplaceGroomingAndHygieneResponse>(product);
        var query = new ReplaceProductResponseQuery { Product = product };
        IQueryHandler<ReplaceProductResponseQuery, ReplaceProductResponse> sut = GetReplaceProductResponseHandler();

        ReplaceProductResponse? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ReplaceGroomingAndHygieneResponse>();
        result.ShouldBeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_ReturnsBaseResponse_WhenCategoryIsNotSpecialized()
    {
        var product = new ProductTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        var expectedResponse = Mapper.Map<ReplaceProductResponse>(product);
        var query = new ReplaceProductResponseQuery { Product = product };
        IQueryHandler<ReplaceProductResponseQuery, ReplaceProductResponse> sut = GetReplaceProductResponseHandler();

        ReplaceProductResponse? result = await sut.Handle(query);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ReplaceProductResponse>();
        result.ShouldBeEquivalentTo(expectedResponse);
    }
}
