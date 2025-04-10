// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Review.ApplicationLayer.Commands.UpdateReview;
using backend.Review.ApplicationLayer.Queries.GetReview;
using backend.Review.Controllers;
using backend.Review.Services;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class UpdateReviewControllerTestBase
{
    protected readonly IMapper Mapper;
    protected readonly IQueryHandler<GetReviewQuery, DomainModels.Review?> MockGetReviewHandler;
    protected readonly ICommandHandler<UpdateReviewCommand> MockUpdateReviewHandler;
    protected readonly Fixture Fixture = new();

    protected UpdateReviewControllerTestBase()
    {
        MockGetReviewHandler = Mock.Of<IQueryHandler<GetReviewQuery, DomainModels.Review?>>();
        MockUpdateReviewHandler = Mock.Of<ICommandHandler<UpdateReviewCommand>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected UpdateReviewController UpdateReviewController()
    {
        return new UpdateReviewController(
            MockGetReviewHandler,
            MockUpdateReviewHandler,
            Mapper);
    }
}
