// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.ReviewControllers;
using backend.Review.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Review.Tests.ControllerTests;

public class DeleteReviewControllerTests : DeleteReviewControllerTestBase
{
    [Fact]
    public async Task DeleteReview_ReviewExists_ReturnsNoContent()
    {
        DomainModels.Review review = new ReviewTestDataBuilder().Build();
        Mock
            .Get(MockGetReviewHandler)
            .Setup(svc => svc.Handle(It.Is<GetReviewQuery>(q => q.Id == review.Id)))
            .ReturnsAsync(review);
        DeleteReviewController sut = DeleteReviewController();

        ActionResult result = await sut.DeleteReview(review.Id, new DeleteReviewRequest());

        result.ShouldBeOfType<NoContentResult>();
        Mock
            .Get(MockDeleteReviewHandler)
            .Verify(svc => svc.Handle(new DeleteReviewCommand { Id = review.Id }), Times.Once);
    }

    [Fact]
    public async Task DeleteReview_ReviewDoesNotExist_ReturnsNotFound()
    {
        DomainModels.Review review = new ReviewTestDataBuilder().Build();
        Mock
            .Get(MockGetReviewHandler)
            .Setup(svc => svc.Handle(It.Is<GetReviewQuery>(q => q.Id == review.Id)))
            .ReturnsAsync((DomainModels.Review?)null);
        DeleteReviewController sut = DeleteReviewController();

        ActionResult result = await sut.DeleteReview(review.Id, new DeleteReviewRequest());

        result.ShouldBeOfType<NotFoundResult>();
        Mock
            .Get(MockDeleteReviewHandler)
            .Verify(svc => svc.Handle(new DeleteReviewCommand { Id = review.Id }), Times.Never);
    }
}
