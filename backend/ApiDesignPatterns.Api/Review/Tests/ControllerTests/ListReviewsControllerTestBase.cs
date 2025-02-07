// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using AutoMapper;
using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Review.DomainModels;
using backend.Review.ReviewControllers;
using backend.Review.Services;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class ListReviewsControllerTestBase
{
    protected readonly IQueryHandler<ListReviewsQuery, (List<ReviewView>, string?)> MockListReviews;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;
    protected Fixture Fixture = new();

    protected ListReviewsControllerTestBase()
    {
        MockListReviews = Mock.Of<IQueryHandler<ListReviewsQuery, (List<ReviewView>, string?)>>();
        MapperConfiguration mapperConfiguration = new(cfg => { cfg.AddProfile<ReviewMappingProfile>(); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    protected ListReviewsController ListReviewsController()
    {
        return new ListReviewsController(MockListReviews, _mapper);
    }
}
