// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.MatchProductToUpdateRequest;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class MatchProductToUpdateRequestHandlerTests : MatchProductToUpdateRequestTestBase
{
    [Fact]
    public void Handle_ReturnsEmptyCollection_WhenNoMatchesExist()
    {
        var existingProducts = new List<DomainModels.Product>
        {
            new ProductTestDataBuilder().WithId(1).Build(), new ProductTestDataBuilder().WithId(2).Build()
        };
        var updateRequests = new List<UpdateProductRequestWithId> { new() { Id = 3 }, new() { Id = 4 } };
        var query = new MatchProductToUpdateRequestQuery
        {
            ExistingProducts = existingProducts, UpdateProductRequests = updateRequests
        };
        var sut = GetMatchProductToUpdateRequestHandler();

        var result = sut.Handle(query);

        result.Should().NotBeNull();
        result.MatchedPairs.Should().BeEmpty();
    }

    [Fact]
    public void Handle_ReturnsAllMatches_WhenMatchesExist()
    {
        var product1 = new ProductTestDataBuilder().WithId(1).Build();
        var product2 = new ProductTestDataBuilder().WithId(2).Build();
        var product3 = new ProductTestDataBuilder().WithId(3).Build();
        var existingProducts = new List<DomainModels.Product> { product1, product2, product3 };
        var updateRequest1 = new UpdateProductRequestWithId { Id = 1 };
        var updateRequest2 = new UpdateProductRequestWithId { Id = 3 };
        var updateRequests = new List<UpdateProductRequestWithId> { updateRequest1, updateRequest2 };
        var query = new MatchProductToUpdateRequestQuery
        {
            ExistingProducts = existingProducts, UpdateProductRequests = updateRequests
        };
        var sut = GetMatchProductToUpdateRequestHandler();

        var result = sut.Handle(query);

        result.Should().NotBeNull();
        result.MatchedPairs.Should().HaveCount(2);
        result.MatchedPairs.Should().Contain(pair => pair.ExistingProduct.Id == 1 && pair.RequestProduct.Id == 1);
        result.MatchedPairs.Should().Contain(pair => pair.ExistingProduct.Id == 3 && pair.RequestProduct.Id == 3);
    }

    [Fact]
    public void Handle_ReturnsOnlyMatchingPairs_WhenPartialMatchesExist()
    {
        var product1 = new ProductTestDataBuilder().WithId(1).Build();
        var product2 = new ProductTestDataBuilder().WithId(2).Build();
        var product3 = new ProductTestDataBuilder().WithId(3).Build();
        var existingProducts = new List<DomainModels.Product> { product1, product2, product3 };
        var updateRequest1 = new UpdateProductRequestWithId { Id = 1 };
        var updateRequest2 = new UpdateProductRequestWithId { Id = 4 };
        var updateRequests = new List<UpdateProductRequestWithId> { updateRequest1, updateRequest2 };
        var query = new MatchProductToUpdateRequestQuery
        {
            ExistingProducts = existingProducts, UpdateProductRequests = updateRequests
        };
        var sut = GetMatchProductToUpdateRequestHandler();

        var result = sut.Handle(query);

        result.Should().NotBeNull();
        result.MatchedPairs.Should().HaveCount(1);
        result.MatchedPairs.Should().Contain(pair => pair.ExistingProduct.Id == 1 && pair.RequestProduct.Id == 1);
        result.MatchedPairs.Should().NotContain(pair => pair.ExistingProduct.Id == 4 || pair.RequestProduct.Id == 4);
    }

    [Fact]
    public void Handle_PreservesProductData_WhenMatchingPairs()
    {
        var product = new ProductTestDataBuilder()
            .WithId(1)
            .WithName(new Name("Original Product"))
            .Build();
        var existingProducts = new List<DomainModels.Product> { product };
        var updateRequest = new UpdateProductRequestWithId { Id = 1, Name = "Updated Product" };
        var updateRequests = new List<UpdateProductRequestWithId> { updateRequest };
        var query = new MatchProductToUpdateRequestQuery
        {
            ExistingProducts = existingProducts, UpdateProductRequests = updateRequests
        };
        var sut = GetMatchProductToUpdateRequestHandler();

        var result = sut.Handle(query);

        result.Should().NotBeNull();
        result.MatchedPairs.Should().HaveCount(1);
        var pair = result.MatchedPairs.First();
        pair.ExistingProduct.Should().Be(product);
        pair.ExistingProduct.Name.Value.Should().Be("Original Product");
        pair.RequestProduct.Should().Be(updateRequest);
        pair.RequestProduct.Name.Should().Be("Updated Product");
    }
}
