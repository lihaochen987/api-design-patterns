// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Commands.UpdateReview;
using backend.Review.Controllers;
using backend.Review.DomainModels.ValueObjects;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class UpdateReviewHandlerTests : UpdateReviewHandlerTestBase
{
    [Fact]
    public async Task Handle_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Review review = new ReviewTestDataBuilder()
            .WithId(3)
            .WithProductId(42)
            .WithRating(new Rating(3.5m))
            .WithText(new Text("Original review text"))
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            ProductId = "55",
            Rating = 4.5m,
            Text = "Updated review text",
            FieldMask = ["productid", "rating", "text"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().ProductId.Should().Be(55);
        Repository.First().Rating.Value.Should().Be(4.5m);
        Repository.First().Text.Value.Should().Be("Updated review text");
        Repository.First().UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithPartialFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Review review = new ReviewTestDataBuilder()
            .WithId(5)
            .WithProductId(42)
            .WithRating(new Rating(3.5m))
            .WithText(new Text("Original review text"))
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            ProductId = "55", Rating = 4.5m, Text = "Updated review text", FieldMask = ["rating"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().ProductId.Should().Be(42);
        Repository.First().Rating.Value.Should().Be(4.5m);
        Repository.First().Text.Value.Should().Be("Original review text");
        Repository.First().UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithEmptyFieldValues_ShouldNotUpdateFields()
    {
        DomainModels.Review review = new ReviewTestDataBuilder()
            .WithId(7)
            .WithProductId(42)
            .WithRating(new Rating(3.5m))
            .WithText(new Text("Original review text"))
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            ProductId = "", Rating = null, Text = "", FieldMask = ["productid", "rating", "text"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().ProductId.Should().Be(42);
        Repository.First().Rating.Value.Should().Be(3.5m);
        Repository.First().Text.Value.Should().Be("Original review text");
        Repository.First().UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithCaseInsensitiveFieldMask_ShouldUpdateFields()
    {
        DomainModels.Review review = new ReviewTestDataBuilder()
            .WithId(9)
            .WithRating(new Rating(3.5m))
            .WithText(new Text("Original review text"))
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            Rating = 4.5m, Text = "Updated review text", FieldMask = ["RATING", "TEXT"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Rating.Value.Should().Be(4.5m);
        Repository.First().Text.Value.Should().Be("Updated review text");
        Repository.First().UpdatedAt.Should().NotBeNull();
    }
}
