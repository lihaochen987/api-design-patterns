// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.Controllers;
using backend.Review.Services;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class ReplaceReviewControllerTestBase
{
    protected readonly IAsyncQueryHandler<GetReviewQuery, DomainModels.Review?> GetReview =
        Mock.Of<IAsyncQueryHandler<GetReviewQuery, DomainModels.Review?>>();

    protected readonly ICommandHandler<ReplaceReviewCommand> ReplaceReview =
        Mock.Of<ICommandHandler<ReplaceReviewCommand>>();

    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ReplaceReviewControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        ReviewMappingConfig.RegisterReviewMappings(config);
        Mapper = new Mapper(config);
    }

    protected ReplaceReviewController GetReplaceReviewController()
    {
        return new ReplaceReviewController(
            GetReview,
            ReplaceReview,
            Mapper);
    }
}
