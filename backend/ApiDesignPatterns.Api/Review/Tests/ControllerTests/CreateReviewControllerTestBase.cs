// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Commands.CreateReview;
using backend.Review.Controllers;
using backend.Review.Services;
using backend.Shared.CommandHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class CreateReviewControllerTestBase
{
    protected readonly ICommandHandler<CreateReviewCommand> CreateReview =
        Mock.Of<ICommandHandler<CreateReviewCommand>>();

    protected readonly IMapper Mapper;

    protected readonly Fixture Fixture = new();

    protected CreateReviewControllerTestBase()
    {
        var config = new TypeAdapterConfig();
        ReviewMappingConfig.RegisterReviewMappings(config);
        Mapper = new Mapper(config);
    }

    protected CreateReviewController GetCreateReviewController()
    {
        return new CreateReviewController(
            CreateReview,
            Mapper);
    }
}
