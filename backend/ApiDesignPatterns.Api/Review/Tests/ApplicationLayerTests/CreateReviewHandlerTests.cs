// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class CreateReviewHandlerTests : CreateReviewHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectReview()
    {
        var reviewToCreate = new ReviewTestDataBuilder().Build();
        long productId = Fixture.Create<long>();
        ICommandHandler<CreateReviewCommand> sut = CreateReviewService();

        await sut.Handle(new CreateReviewCommand { Review = reviewToCreate, ProductId = productId });

        Repository.IsDirty.Should().BeTrue();
        Repository.CallCount.Should().ContainKey("CreateReviewAsync").WhoseValue.Should().Be(1);
        Repository.First().ProductId.Should().Be(productId);
    }

    [Fact]
    public async Task Handle_PersistsWhenCalledTwice()
    {
        var firstReviewToCreate = new ReviewTestDataBuilder().Build();
        var secondReviewToCreate = new ReviewTestDataBuilder().Build();
        long firstProductId = Fixture.Create<long>();
        long secondProductId = Fixture.Create<long>();
        ICommandHandler<CreateReviewCommand> sut = CreateReviewService();

        await sut.Handle(new CreateReviewCommand { Review = firstReviewToCreate, ProductId = firstProductId });
        await sut.Handle(new CreateReviewCommand { Review = secondReviewToCreate, ProductId = secondProductId });

        Repository.IsDirty.Should().BeTrue();
        Repository.CallCount.Should().ContainKey("CreateReviewAsync").WhoseValue.Should().Be(2);
        var firstReview = Repository.First(x => x.Id == firstReviewToCreate.Id);
        firstReview.ProductId.Should().Be(firstProductId);
        var secondReview = Repository.First(x => x.Id == secondReviewToCreate.Id);
        secondReview.ProductId.Should().Be(secondProductId);
    }
}
