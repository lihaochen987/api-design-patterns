// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class GetReviewHandlerTests : GetReviewHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsReview_WhenReviewExists()
    {
        DomainModels.Review expectedReview = new ReviewTestDataBuilder().Build();
        Repository.Add(expectedReview);
        var sut = GetReviewHandler();

        DomainModels.Review? result = await sut.Handle(new GetReviewQuery { Id = expectedReview.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedReview);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenReviewDoesNotExist()
    {
        DomainModels.Review nonExistentReview = new ReviewTestDataBuilder().Build();
        var sut = GetReviewHandler();

        DomainModels.Review? result = await sut.Handle(new GetReviewQuery { Id = nonExistentReview.Id });

        result.Should().BeNull();
    }
}
