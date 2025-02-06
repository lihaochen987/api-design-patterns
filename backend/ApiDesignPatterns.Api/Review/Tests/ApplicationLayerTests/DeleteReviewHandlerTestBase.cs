// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Review.Tests.ApplicationLayerTests;

public abstract class DeleteReviewHandlerTestBase
{
    protected readonly ReviewRepositoryFake Repository = [];

    protected ICommandHandler<DeleteReviewCommand> DeleteReviewService()
    {
        return new DeleteReviewHandler(Repository);
    }
}
