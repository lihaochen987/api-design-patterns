// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.DomainModels.ValueObjects;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class ReplaceReviewHandlerTests : ReplaceReviewHandlerTestBase
{
    [Fact]
    public async Task Handle_UpdatesReview_WhenValidReviewIsProvided()
    {
        var existingReview = new ReviewTestDataBuilder()
            .WithRating(new Rating(3.5m))
            .WithText(new Text("Original review"))
            .Build();
        var replacementReview = new ReviewTestDataBuilder()
            .WithId(existingReview.Id)
            .WithRating(new Rating(4.0m))
            .WithText(new Text("Updated review"))
            .Build();
        Repository.Add(existingReview);
        var command = new ReplaceReviewCommand { Review = replacementReview };
        ICommandHandler<ReplaceReviewCommand> sut = ReplaceReviewHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        var updatedReview = Repository.First();
        updatedReview.Rating.Should().Be(replacementReview.Rating);
        updatedReview.Text.Should().Be(replacementReview.Text);
    }

    [Fact]
    public async Task Handle_SetsTimestamps_WhenReplacingReview()
    {
        DateTimeOffset oldCreatedAt = DateTimeOffset.UtcNow.AddDays(-1);
        var existingReview = new ReviewTestDataBuilder()
            .WithCreatedAt(oldCreatedAt)
            .WithUpdatedAt(DateTimeOffset.UtcNow.AddHours(-1))
            .Build();
        var replacementReview = new ReviewTestDataBuilder()
            .WithId(existingReview.Id)
            .WithCreatedAt(DateTimeOffset.UtcNow.AddDays(-2))
            .WithUpdatedAt(DateTimeOffset.UtcNow)
            .Build();
        Repository.Add(existingReview);
        var command = new ReplaceReviewCommand { Review = replacementReview };
        ICommandHandler<ReplaceReviewCommand> sut = ReplaceReviewHandler();

        await sut.Handle(command);

        var updatedReview = Repository.First();
        updatedReview.CreatedAt.Should().BeAfter(oldCreatedAt);
        updatedReview.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task Handle_UpdatesMultipleReviews_Successfully()
    {
        var firstReview = new ReviewTestDataBuilder().Build();
        var secondReview = new ReviewTestDataBuilder().Build();
        Repository.Add(firstReview);
        Repository.Add(secondReview);
        var replacementReview = new ReviewTestDataBuilder()
            .WithId(firstReview.Id)
            .Build();
        var command = new ReplaceReviewCommand { Review = replacementReview };
        ICommandHandler<ReplaceReviewCommand> sut = ReplaceReviewHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        var updatedReview = Repository.Single(r => r.Id == firstReview.Id);
        updatedReview.Id.Should().Be(replacementReview.Id);
        updatedReview.ProductId.Should().Be(replacementReview.ProductId);
        updatedReview.Rating.Should().Be(replacementReview.Rating);
        updatedReview.Text.Should().Be(replacementReview.Text);
    }
}
