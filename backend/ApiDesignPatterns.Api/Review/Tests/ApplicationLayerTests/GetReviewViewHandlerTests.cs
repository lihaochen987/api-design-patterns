// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.DomainModels;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class GetReviewViewHandlerTests : GetReviewViewHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsNull_WhenReviewDoesNotExist()
    {
        ReviewView expectedReview = new ReviewViewTestDataBuilder().Build();
        IQueryHandler<GetReviewViewQuery, ReviewView?> sut = GetReviewViewHandler();

        ReviewView? result = await sut.Handle(new GetReviewViewQuery { Id = expectedReview.Id });

        result.ShouldBeNull();
    }

    [Fact]
    public async Task Handle_ReturnsReview_WhenReviewExists()
    {
        ReviewView expectedReview = new ReviewViewTestDataBuilder().Build();
        Repository.Add(expectedReview);
        IQueryHandler<GetReviewViewQuery, ReviewView?> sut = GetReviewViewHandler();

        ReviewView? result = await sut.Handle(new GetReviewViewQuery { Id = expectedReview.Id });

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedReview);
    }
}
