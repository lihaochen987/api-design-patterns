// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.UpdateReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.ReviewControllers;
using backend.Review.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Review.Tests.ControllerTests;

public class UpdateReviewControllerTests : UpdateReviewControllerTestBase
{
    [Fact]
    public async Task UpdateReview_WithValidRequest_ShouldReturnUpdatedReview()
    {
        var review = new ReviewTestDataBuilder().Build();
        UpdateReviewRequest request = new() { Rating = 5, Text = "Updated review text" };
        Mock
            .Get(MockGetReviewHandler)
            .Setup(svc => svc.Handle(It.Is<GetReviewQuery>(q => q.Id == review.Id)))
            .ReturnsAsync(review);
        UpdateReviewController sut = UpdateReviewController();

        ActionResult<UpdateReviewResponse> actionResult = await sut.UpdateReview(review.Id, request);

        actionResult.Result.ShouldBeOfType<OkObjectResult>();
        OkObjectResult? contentResult = (OkObjectResult) actionResult.Result;
        UpdateReviewResponse response = (UpdateReviewResponse) contentResult.Value!;
        response.ShouldBeEquivalentTo(Mapper.Map<UpdateReviewResponse>(review));
        Mock
            .Get(MockUpdateReviewHandler)
            .Verify(
                svc => svc.Handle(It.IsAny<UpdateReviewCommand>()),
                Times.Once);
    }

    [Fact]
    public async Task UpdateReview_NonExistentReview_ShouldReturnNotFound()
    {
        UpdateReviewRequest request = new() { Rating = 3, Text = "This review doesn't exist" };
        long nonExistentId = Fixture.Create<long>();
        Mock
            .Get(MockGetReviewHandler)
            .Setup(svc => svc.Handle(It.Is<GetReviewQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.Review?)null);
        var sut = UpdateReviewController();

        ActionResult<UpdateReviewResponse> actionResult = await sut.UpdateReview(nonExistentId, request);

        actionResult.Result.ShouldNotBeNull();
        actionResult.Result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(MockUpdateReviewHandler)
            .Verify(
                svc => svc.Handle(It.IsAny<UpdateReviewCommand>()),
                Times.Never);
    }

    [Fact]
    public async Task UpdateReview_WhenUpdateSucceeds_ShouldReturnMappedResponse()
    {
        var review = new ReviewTestDataBuilder().Build();
        UpdateReviewRequest request = new() { Rating = 4, Text = "Updated review content" };

        Mock
            .Get(MockGetReviewHandler)
            .Setup(svc => svc.Handle(It.Is<GetReviewQuery>(q => q.Id == review.Id)))
            .ReturnsAsync(review);
        var expectedResponse = Mapper.Map<UpdateReviewResponse>(review);
        var sut = UpdateReviewController();

        var result = await sut.UpdateReview(review.Id, request);

        result.ShouldNotBeNull();
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdateReview_WithInvalidRating_ShouldStillCallHandler()
    {
        var review = new ReviewTestDataBuilder().Build();
        UpdateReviewRequest request = new() { Rating = 6, Text = "Updated text" };
        Mock
            .Get(MockGetReviewHandler)
            .Setup(svc => svc.Handle(It.Is<GetReviewQuery>(q => q.Id == review.Id)))
            .ReturnsAsync(review);
        var sut = UpdateReviewController();

        var result = await sut.UpdateReview(review.Id, request);

        result.ShouldNotBeNull();
        Mock
            .Get(MockUpdateReviewHandler)
            .Verify(
                svc => svc.Handle(It.Is<UpdateReviewCommand>(cmd =>
                    cmd.Request == request &&
                    cmd.Review == review)),
                Times.Once);
    }
}
