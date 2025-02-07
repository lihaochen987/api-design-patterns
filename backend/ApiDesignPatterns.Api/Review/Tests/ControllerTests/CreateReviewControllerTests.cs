// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.ReviewControllers;
using backend.Review.Tests.TestHelpers.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Review.Tests.ControllerTests;

public class CreateReviewControllerTests : CreateReviewControllerTestBase
{
    [Fact]
    public async Task CreateReview_ReturnsCreatedResponse_WhenReviewCreatedSuccessfully()
    {
        long productId = Fixture.Create<long>();
        var review = new ReviewTestDataBuilder().Build();
        var request = Mapper.Map<CreateReviewRequest>(review);
        CreateReviewController sut = GetCreateReviewController();

        var result = await sut.CreateReview(request, productId);

        result.ShouldNotBeNull();
        var createdResult = result.Result.ShouldBeOfType<CreatedAtActionResult>();
        createdResult.ActionName.ShouldBe("GetReview");
        createdResult.ControllerName.ShouldBe("GetReview");
        Mock
            .Get(CreateReview)
            .Verify(x => x.Handle(It.Is<CreateReviewCommand>(c =>
                    c.ProductId == productId)),
                Times.Once);
    }

    [Fact]
    public async Task CreateReview_SetsCreatedAtToUtcNow_WhenCreatingReview()
    {
        long productId = Fixture.Create<long>();
        var review = new ReviewTestDataBuilder().Build();
        var request = Mapper.Map<CreateReviewRequest>(review);
        var beforeTest = DateTime.UtcNow;
        var sut = GetCreateReviewController();

        await sut.CreateReview(request, productId);

        Mock
            .Get(CreateReview)
            .Verify(x => x.Handle(It.Is<CreateReviewCommand>(c =>
                    c.Review.CreatedAt >= beforeTest &&
                    c.Review.CreatedAt <= DateTime.UtcNow)),
                Times.Once);
    }

    [Fact]
    public async Task CreateReview_HandlesCommandFailure_WhenCreateReviewFails()
    {
        long productId = Fixture.Create<long>();
        var review = new ReviewTestDataBuilder().Build();
        var request = Mapper.Map<CreateReviewRequest>(review);
        Mock
            .Get(CreateReview)
            .Setup(x => x.Handle(It.IsAny<CreateReviewCommand>()))
            .ThrowsAsync(new Exception("Failed to create review"));
        var sut = GetCreateReviewController();

        await Should.ThrowAsync<Exception>(() => sut.CreateReview(request, productId));
    }
}
