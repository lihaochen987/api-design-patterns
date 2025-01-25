// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Product.ApplicationLayer.Queries.GetProductView;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class GetProductViewHandlerTests : GetProductViewHandlerTestBase
{
    [Fact]
    public async Task GetProductView_ShouldReturnProduct_WhenProductExists()
    {
        var productView = new ProductViewTestDataBuilder().WithName("Sample Product").Build();
        Repository.Add(productView);
        IQueryHandler<GetProductViewQuery, ProductView> sut = GetProductViewHandler();

        ProductView? result = await sut.Handle(new GetProductViewQuery { Id = productView.Id });

        result.ShouldNotBeNull();
        result.ShouldBe(productView);
    }

    [Fact]
    public async Task GetProductView_ShouldReturnNull_WhenProductDoesNotExist()
    {
        long productId = Fixture.Create<long>();
        IQueryHandler<GetProductViewQuery, ProductView> sut = GetProductViewHandler();

        ProductView? result = await sut.Handle(new GetProductViewQuery { Id = productId });

        result.ShouldBeNull();
    }
}
