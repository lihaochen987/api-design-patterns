// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Review.Tests.ApplicationLayerTests;

public abstract class CreateReviewHandlerTestBase
{
    protected readonly Fixture Fixture = new();
    protected readonly ReviewRepositoryFake Repository = [];

    protected ICommandHandler<CreateReviewCommand> CreateReviewService()
    {
        return new CreateReviewHandler(Repository);
    }
}
