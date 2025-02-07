// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Linq.Expressions;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared;

namespace backend.Review.Tests.TestHelpers.Fakes;

public class ReviewViewRepositoryFake(
    QueryService<ReviewView> queryService)
    : Collection<ReviewView>, IReviewViewRepository
{
    public void AddReviewView(long id, long productId, decimal rating, string text)
    {
        var reviewView = new ReviewViewTestDataBuilder()
            .WithId(id)
            .WithProductId(productId)
            .WithRating(rating)
            .WithText(text)
            .Build();
        Add(reviewView);
    }

    public Task<ReviewView?> GetReviewView(long id)
    {
        ReviewView? reviewView = this.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(reviewView);
    }

    public Task<(List<ReviewView>, string?)> ListReviewsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent)
    {
        IEnumerable<ReviewView> query = this.AsEnumerable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenReviewId))
        {
            query = query.Where(r => r.Id > lastSeenReviewId);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            Expression<Func<ReviewView, bool>> filterExpression = queryService.BuildFilterExpression(filter);
            query = query.Where(filterExpression.Compile());
        }

        List<ReviewView> reviews = query.OrderBy(r => r.Id).ToList();

        List<ReviewView> paginatedReviews = queryService.Paginate(reviews, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedReviews, nextPageToken));
    }
}
