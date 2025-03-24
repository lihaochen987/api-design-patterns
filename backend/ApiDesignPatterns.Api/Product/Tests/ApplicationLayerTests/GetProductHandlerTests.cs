// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class GetProductHandlerTests : GetProductHandlerTestBase
{
    [Fact]
    public async Task GetProductAsync_ReturnsProduct_WhenProductExists()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        Repository.Add(expectedProduct);
        IQueryHandler<GetProductQuery, DomainModels.Product?> sut = GetProductHandler();

        DomainModels.Product? result = await sut.Handle(new GetProductQuery { Id = expectedProduct.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task GetProductAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        DomainModels.Product expectedProduct = new ProductTestDataBuilder().Build();
        IQueryHandler<GetProductQuery, DomainModels.Product?> sut = GetProductHandler();

        DomainModels.Product? result = await sut.Handle(new GetProductQuery { Id = expectedProduct.Id });

        result.Should().BeNull();
    }
}
