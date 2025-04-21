// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.DomainModels.Views;
using backend.Product.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Product.Tests.ApplicationLayerTests;

public class GetProductPricingHandlerTests : GetProductPricingHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsProductPricing_WhenProductPricingExists()
    {
        ProductPricingView expectedPricing = new ProductPricingViewTestDataBuilder().Build();
        Repository.Add(expectedPricing);
        var sut = GetProductPricingHandler();

        ProductPricingView? result = await sut.Handle(new GetProductPricingQuery { Id = expectedPricing.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPricing);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenProductPricingDoesNotExist()
    {
        ProductPricingView expectedPricing = new ProductPricingViewTestDataBuilder().Build();
        var sut = GetProductPricingHandler();

        ProductPricingView? result = await sut.Handle(new GetProductPricingQuery { Id = expectedPricing.Id });

        result.Should().BeNull();
    }
}
