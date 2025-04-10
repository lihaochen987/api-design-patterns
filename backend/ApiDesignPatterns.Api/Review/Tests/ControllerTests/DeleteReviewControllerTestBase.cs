// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ApplicationLayer.Commands.DeleteReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.Controllers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class DeleteReviewControllerTestBase
{
    protected readonly IQueryHandler<GetReviewQuery, DomainModels.Review?> MockGetReviewHandler =
        Mock.Of<IQueryHandler<GetReviewQuery, DomainModels.Review?>>();

    protected readonly ICommandHandler<DeleteReviewCommand> MockDeleteReviewHandler =
        Mock.Of<ICommandHandler<DeleteReviewCommand>>();

    protected DeleteReviewController DeleteReviewController()
    {
        return new DeleteReviewController(MockGetReviewHandler, MockDeleteReviewHandler);
    }
}
