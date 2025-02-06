// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Commands.UpdateReview;
using backend.Review.ReviewControllers;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using Shouldly;
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
            .WithRating(3.5m)
            .WithText("Original review text")
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            ProductId = "55",
            Rating = "4.5",
            Text = "Updated review text",
            FieldMask = ["productid", "rating", "text"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateReviewAsync", 1);
        Repository.First().ProductId.ShouldBe(55);
        Repository.First().Rating.ShouldBe(4.5m);
        Repository.First().Text.ShouldBe("Updated review text");
        Repository.First().UpdatedAt.ShouldNotBeNull();
    }

    [Fact]
    public async Task Handle_WithPartialFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        DomainModels.Review review = new ReviewTestDataBuilder()
            .WithId(5)
            .WithProductId(42)
            .WithRating(3.5m)
            .WithText("Original review text")
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            ProductId = "55", Rating = "4.5", Text = "Updated review text", FieldMask = ["rating"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateReviewAsync", 1);
        Repository.First().ProductId.ShouldBe(42);
        Repository.First().Rating.ShouldBe(4.5m);
        Repository.First().Text.ShouldBe("Original review text");
        Repository.First().UpdatedAt.ShouldNotBeNull();
    }

    [Fact]
    public async Task Handle_WithEmptyFieldValues_ShouldNotUpdateFields()
    {
        DomainModels.Review review = new ReviewTestDataBuilder()
            .WithId(7)
            .WithProductId(42)
            .WithRating(3.5m)
            .WithText("Original review text")
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            ProductId = "", Rating = "", Text = "", FieldMask = ["productid", "rating", "text"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateReviewAsync", 1);
        Repository.First().ProductId.ShouldBe(42);
        Repository.First().Rating.ShouldBe(3.5m);
        Repository.First().Text.ShouldBe("Original review text");
        Repository.First().UpdatedAt.ShouldNotBeNull();
    }

    [Fact]
    public async Task Handle_WithCaseInsensitiveFieldMask_ShouldUpdateFields()
    {
        DomainModels.Review review = new ReviewTestDataBuilder()
            .WithId(9)
            .WithRating(3.5m)
            .WithText("Original review text")
            .Build();
        Repository.Add(review);
        Repository.IsDirty = false;
        UpdateReviewRequest request = new()
        {
            Rating = "4.5", Text = "Updated review text", FieldMask = ["RATING", "TEXT"]
        };
        ICommandHandler<UpdateReviewCommand> sut = UpdateReviewService();

        await sut.Handle(new UpdateReviewCommand { Review = review, Request = request });

        Repository.IsDirty.ShouldBeTrue();
        Repository.CallCount.ShouldContainKeyAndValue("UpdateReviewAsync", 1);
        Repository.First().Rating.ShouldBe(4.5m);
        Repository.First().Text.ShouldBe("Updated review text");
        Repository.First().UpdatedAt.ShouldNotBeNull();
    }
}
