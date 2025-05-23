// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.Controllers;
using backend.Review.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

        result.Should().NotBeNull();
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be("GetReview");
        createdResult.ControllerName.Should().Be("GetReview");
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

        Func<Task> act = async () => await sut.CreateReview(request, productId);
        await act.Should().ThrowAsync<Exception>();
    }
}
