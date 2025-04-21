// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Review.ApplicationLayer.Queries.GetReviewView;
using backend.Review.Controllers;
using backend.Review.DomainModels;
using backend.Review.Services;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class GetReviewControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IAsyncQueryHandler<GetReviewViewQuery, ReviewView?> MockGetReviewView =
        Mock.Of<IAsyncQueryHandler<GetReviewViewQuery, ReviewView?>>();

    protected readonly IMapper Mapper;

    private readonly IFieldMaskConverterFactory _fieldMaskConverterFactory =
        new FieldMaskConverterFactory(new ReviewFieldPaths().ValidPaths);

    protected GetReviewControllerTestBase()
    {
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    protected GetReviewController GetReviewController()
    {
        return new GetReviewController(
            MockGetReviewView,
            _fieldMaskConverterFactory,
            Mapper);
    }
}
