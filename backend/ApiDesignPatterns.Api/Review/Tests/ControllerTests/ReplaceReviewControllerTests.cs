// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.ReviewControllers;
using backend.Review.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Review.Tests.ControllerTests;

public class ReplaceReviewControllerTests : ReplaceReviewControllerTestBase
{
    [Fact]
    public async Task ReplaceReview_ReturnsOkResponse_WhenReviewReplacedSuccessfully()
    {
        var review = new ReviewTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceReviewRequest>(review);
        var expectedResponse = Mapper.Map<ReplaceReviewResponse>(review);
        Mock
            .Get(GetReview)
            .Setup(x => x.Handle(It.Is<GetReviewQuery>(q => q.Id == review.Id)))
            .ReturnsAsync(review);
        ReplaceReviewController sut = GetReplaceReviewController();

        var result = await sut.ReplaceReview(review.Id, request);

        result.ShouldNotBeNull();
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBeEquivalentTo(expectedResponse);
        Mock
            .Get(ReplaceReview)
            .Verify(x => x.Handle(It.IsAny<ReplaceReviewCommand>()), Times.Once);
    }

    [Fact]
    public async Task ReplaceReview_ReturnsNotFound_WhenReviewDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        var review = new ReviewTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceReviewRequest>(review);
        Mock
            .Get(GetReview)
            .Setup(x => x.Handle(It.Is<GetReviewQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.Review?)null);
        ReplaceReviewController sut = GetReplaceReviewController();

        var result = await sut.ReplaceReview(nonExistentId, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(ReplaceReview)
            .Verify(x => x.Handle(It.IsAny<ReplaceReviewCommand>()), Times.Never);
    }
}
