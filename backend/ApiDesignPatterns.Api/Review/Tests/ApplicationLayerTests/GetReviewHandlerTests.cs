// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class GetReviewHandlerTests : GetReviewHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsReview_WhenReviewExists()
    {
        DomainModels.Review expectedReview = new ReviewTestDataBuilder().Build();
        Repository.Add(expectedReview);
        IQueryHandler<GetReviewQuery, DomainModels.Review> sut = GetReviewHandler();

        DomainModels.Review? result = await sut.Handle(new GetReviewQuery { Id = expectedReview.Id });

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedReview);
        Repository.CallCount.ShouldContainKeyAndValue("GetReviewAsync", 1);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenReviewDoesNotExist()
    {
        DomainModels.Review nonExistentReview = new ReviewTestDataBuilder().Build();
        IQueryHandler<GetReviewQuery, DomainModels.Review> sut = GetReviewHandler();

        DomainModels.Review? result = await sut.Handle(new GetReviewQuery { Id = nonExistentReview.Id });

        result.ShouldBeNull();
        Repository.CallCount.ShouldContainKeyAndValue("GetReviewAsync", 1);
    }
}
