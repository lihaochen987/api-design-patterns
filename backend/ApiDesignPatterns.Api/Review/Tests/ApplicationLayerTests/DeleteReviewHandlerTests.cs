// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Review.Tests.ApplicationLayerTests;

public class DeleteReviewHandlerTests : DeleteReviewHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectReview()
    {
        DomainModels.Review reviewToDelete = new ReviewTestDataBuilder().Build();
        Repository.Add(reviewToDelete);
        ICommandHandler<DeleteReviewCommand> sut = DeleteReviewService();

        await sut.Handle(new DeleteReviewCommand { Id = reviewToDelete.Id });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DoesNotThrowException_WhenReviewDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        ICommandHandler<DeleteReviewCommand> sut = DeleteReviewService();

        await sut.Handle(new DeleteReviewCommand { Id = nonExistentId });

        Repository.IsDirty.Should().BeFalse();
        Repository.Should().BeEmpty();
    }
}
