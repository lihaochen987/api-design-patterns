// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared.CommandHandler;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class CreateReviewControllerTestBase
{
    protected readonly ICommandHandler<CreateReviewCommand> CreateReview =
        Mock.Of<ICommandHandler<CreateReviewCommand>>();

    protected readonly IMapper Mapper;

    protected Fixture Fixture = new();

    protected CreateReviewControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected CreateReviewController GetCreateReviewController()
    {
        return new CreateReviewController(
            CreateReview,
            Mapper);
    }
}
