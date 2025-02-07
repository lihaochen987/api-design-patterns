// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Review.ApplicationLayer.Commands.ReplaceReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class ReplaceReviewControllerTestBase
{
    protected readonly IQueryHandler<GetReviewQuery, DomainModels.Review> GetReview =
        Mock.Of<IQueryHandler<GetReviewQuery, DomainModels.Review>>();

    protected readonly ICommandHandler<ReplaceReviewCommand> ReplaceReview =
        Mock.Of<ICommandHandler<ReplaceReviewCommand>>();

    protected readonly IMapper Mapper;
    protected readonly Fixture Fixture = new();

    protected ReplaceReviewControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected ReplaceReviewController GetReplaceReviewController()
    {
        return new ReplaceReviewController(
            GetReview,
            ReplaceReview,
            Mapper);
    }
}
