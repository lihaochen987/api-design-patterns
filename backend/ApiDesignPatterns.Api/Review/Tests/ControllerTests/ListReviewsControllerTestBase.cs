// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Review.Controllers;
using backend.Review.Services;
using backend.Shared.QueryHandler;
using Mapster;
using MapsterMapper;
using Moq;

namespace backend.Review.Tests.ControllerTests;

public abstract class ListReviewsControllerTestBase
{
    protected readonly IAsyncQueryHandler<ListReviewsQuery, PagedReviews> MockListReviews;
    private readonly IMapper _mapper;
    protected const int DefaultMaxPageSize = 10;
    protected readonly Fixture Fixture = new();

    protected ListReviewsControllerTestBase()
    {
        MockListReviews = Mock.Of<IAsyncQueryHandler<ListReviewsQuery, PagedReviews>>();
        var config = new TypeAdapterConfig();
        ReviewMappingConfig.RegisterReviewMappings(config);
        _mapper = new Mapper(config);
    }

    protected ListReviewsController ListReviewsController()
    {
        return new ListReviewsController(MockListReviews, _mapper);
    }
}
