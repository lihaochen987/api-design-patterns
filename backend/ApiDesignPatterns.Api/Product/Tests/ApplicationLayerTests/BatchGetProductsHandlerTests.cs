// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.BatchGetProductResponses;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class BatchGetProductsHandlerTests : BatchGetProductsHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsSuccess_WhenAllProductsExist()
    {
        (long productId1, long productId2) = (Fixture.Create<long>(), Fixture.Create<long>());
        Repository.AddProductView(productId1);
        Repository.AddProductView(productId2);
        var query = new BatchGetProductResponsesQuery { ProductIds = [productId1, productId2] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenAnyProductDoesNotExist()
    {
        (long productId1, long productId2) = (Fixture.Create<long>(), Fixture.Create<long>());
        Repository.AddProductView(productId1);
        var query = new BatchGetProductResponsesQuery { ProductIds = [productId1, productId2] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain(productId2.ToString());
    }

    [Theory]
    [InlineData(Category.PetFood, typeof(GetPetFoodResponse))]
    [InlineData(Category.GroomingAndHygiene, typeof(GetGroomingAndHygieneResponse))]
    [InlineData(Category.Clothing, typeof(GetProductResponse))]
    public async Task Handle_MapsToCorrectType_BasedOnCategory(Category category, Type expectedResponseType)
    {
        long productId = Fixture.Create<long>();
        Repository.AddProductView(productId, category);
        var query = new BatchGetProductResponsesQuery { ProductIds = [productId] };
        var sut = GetBatchGetProductsHandler();

        var result = await sut.Handle(query);

        result.IsSuccess.Should().BeTrue();
        result.Value!.First().Should().BeOfType(expectedResponseType);
    }
}
