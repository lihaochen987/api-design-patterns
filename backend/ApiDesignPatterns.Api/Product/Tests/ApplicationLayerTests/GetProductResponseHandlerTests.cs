// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.GetProductResponse;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class GetProductResponseHandlerTests : GetProductResponseHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsNull_WhenProductDoesNotExist()
    {
        ProductView expectedProduct = new ProductViewTestDataBuilder().Build();
        IQueryHandler<GetProductResponseQuery, GetProductResponse?> sut = GetProductResponseHandler();

        GetProductResponse? result =
            await sut.Handle(new GetProductResponseQuery { Id = expectedProduct.Id });

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsPetFoodResponse_WhenCategoryIsPetFood()
    {
        ProductView productView = new ProductViewTestDataBuilder()
            .WithCategory(Category.PetFood)
            .Build();
        Repository.Add(productView);
        IQueryHandler<GetProductResponseQuery, GetProductResponse?> sut = GetProductResponseHandler();
        GetPetFoodResponse expectedResponse = Mapper.Map<GetPetFoodResponse>(productView);

        GetProductResponse? result =
            await sut.Handle(new GetProductResponseQuery { Id = productView.Id });

        result.Should().NotBeNull();
        result.Should().BeOfType<GetPetFoodResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_ReturnsGroomingResponse_WhenCategoryIsGroomingAndHygiene()
    {
        ProductView productView = new ProductViewTestDataBuilder()
            .WithCategory(Category.GroomingAndHygiene)
            .Build();
        GetGroomingAndHygieneResponse expectedResponse = Mapper.Map<GetGroomingAndHygieneResponse>(productView);
        Repository.Add(productView);
        IQueryHandler<GetProductResponseQuery, GetProductResponse?> sut = GetProductResponseHandler();

        GetProductResponse? result =
            await sut.Handle(new GetProductResponseQuery { Id = productView.Id });

        result.Should().NotBeNull();
        result.Should().BeOfType<GetGroomingAndHygieneResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Handle_ReturnsBaseResponse_WhenCategoryIsNotSpecialized()
    {
        ProductView productView = new ProductViewTestDataBuilder()
            .WithCategory(Category.Beds)
            .Build();
        GetProductResponse expectedResponse = Mapper.Map<GetProductResponse>(productView);
        Repository.Add(productView);
        IQueryHandler<GetProductResponseQuery, GetProductResponse?> sut = GetProductResponseHandler();

        GetProductResponse? result =
            await sut.Handle(new GetProductResponseQuery { Id = productView.Id });

        result.Should().NotBeNull();
        result.Should().BeOfType<GetProductResponse>();
        result.Should().BeEquivalentTo(expectedResponse);
    }
}
