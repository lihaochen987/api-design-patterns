// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Shared.QueryHandler;

namespace backend.Review.ApplicationLayer.Queries.ListReviews;

public class ListReviewsHandler(IReviewViewRepository repository)
    : IQueryHandler<ListReviewsQuery, PagedReviews>
{
    public async Task<PagedReviews?> Handle(ListReviewsQuery query)
    {
        (List<ReviewView> reviews, string? nextPageToken) = await repository.ListReviewsAsync(
            query.Request.PageToken,
            query.Request.Filter,
            query.Request.MaxPageSize,
            query.ParentId);
        return new PagedReviews(reviews, nextPageToken);
    }
}
