// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using System.Net;
using backend.Product.Controllers.Product;
using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.Controllers;
using backend.Review.DomainModels;
using backend.Review.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Review.Tests.ControllerTests;

public class GetReviewControllerTests : GetReviewControllerTestBase
{
    [Fact]
    public async Task GetReview_ReturnsOkResult_WhenReviewExists()
    {
        long reviewId = Fixture.Create<long>();
        var reviewView = new ReviewViewTestDataBuilder()
            .WithId(reviewId)
            .WithText("Great product!")
            .WithRating(5)
            .Build();
        var request = Fixture.Build<GetReviewRequest>()
            .With(r => r.FieldMask, ["Text", "Rating"])
            .Create();
        Mock
            .Get(MockGetReviewView)
            .Setup(service => service.Handle(It.Is<GetReviewViewQuery>(q => q.Id == reviewId)))
            .ReturnsAsync(reviewView);
        GetReviewController sut = GetReviewController();

        ActionResult<GetProductResponse> result = await sut.GetReview(reviewId, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = ((string)okResult.Value);
        jsonResult.Should().Contain("Great product!");
    }

    [Fact]
    public async Task GetReview_ReturnsNotFound_WhenReviewDoesNotExist()
    {
        long reviewId = Fixture.Create<long>();
        var request = Fixture.Create<GetReviewRequest>();
        Mock
            .Get(MockGetReviewView)
            .Setup(service => service.Handle(It.Is<GetReviewViewQuery>(q => q.Id == reviewId)))
            .ReturnsAsync((ReviewView?)null);
        GetReviewController sut = GetReviewController();

        ActionResult<GetProductResponse> result = await sut.GetReview(reviewId, request);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetReview_SerializesWithFieldMaskCorrectly()
    {
        long reviewId = Fixture.Create<long>();
        var reviewView = new ReviewViewTestDataBuilder()
            .WithId(reviewId)
            .WithText("Masked Review")
            .WithRating(4)
            .Build();
        var request = Fixture.Build<GetReviewRequest>()
            .With(r => r.FieldMask, ["Text"])
            .Create();
        Mock
            .Get(MockGetReviewView)
            .Setup(service => service.Handle(It.Is<GetReviewViewQuery>(q => q.Id == reviewId)))
            .ReturnsAsync(reviewView);
        GetReviewController sut = GetReviewController();

        ActionResult<GetProductResponse> result = await sut.GetReview(reviewId, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = ((string)okResult.Value);
        jsonResult.Should().Contain("Masked Review");
        jsonResult.Should().NotContain("Rating");
    }
}
