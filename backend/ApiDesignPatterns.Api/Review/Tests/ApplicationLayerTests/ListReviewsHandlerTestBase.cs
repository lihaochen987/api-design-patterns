// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Review.ApplicationLayer.Queries.ListReviews;
using backend.Review.DomainModels;
using backend.Review.Tests.TestHelpers.Fakes;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Review.Tests.ApplicationLayerTests;

public abstract class ListReviewsHandlerTestBase
{
    protected readonly ReviewViewRepositoryFake Repository = new(new QueryService<ReviewView>());
    protected readonly Fixture Fixture = new();

    protected IQueryHandler<ListReviewsQuery, PagedReviews> ListReviewsViewHandler()
    {
        return new ListReviewsHandler(Repository);
    }
}
