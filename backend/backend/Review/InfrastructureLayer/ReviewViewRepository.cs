// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Linq.Expressions;
using backend.Review.DomainModels.Views;
using backend.Review.InfrastructureLayer.Database;
using backend.Shared;
using backend.Shared.CelSpecParser;
using Microsoft.EntityFrameworkCore;

namespace backend.Review.InfrastructureLayer;

public class ReviewViewRepository(ReviewDbContext context) : IReviewViewRepository
{
    public async Task<ReviewView?> GetReviewView(long id) =>
        await context.Set<ReviewView>()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<(List<ReviewView>, string?)> ListReviewsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        IQueryable<ReviewView> query = context.Set<ReviewView>().AsQueryable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenReview))
        {
            query = query.Where(p => p.Id > lastSeenReview);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            Expression<Func<ReviewView, bool>> filterExpression = BuildFilterExpression(filter);
            query = query.Where(filterExpression);
        }

        List<ReviewView> reviews = await query
            .OrderBy(p => p.Id)
            .Take(maxPageSize + 1)
            .ToListAsync();

        List<ReviewView> paginatedReviews = PaginateReviews(reviews, maxPageSize, out string? nextPageToken);

        return (paginatedReviews, nextPageToken);
    }

    private static Expression<Func<ReviewView, bool>> BuildFilterExpression(string filter)
    {
        CelParser<ReviewView> parser = new(new TypeParser());
        List<CelToken> tokens = parser.Tokenize(filter);
        return parser.ParseFilter(tokens);
    }

    private static List<ReviewView> PaginateReviews(
        List<ReviewView> existingReviews,
        int maxPageSize,
        out string? nextPageToken)
    {
        if (existingReviews.Count <= maxPageSize)
        {
            nextPageToken = null;
            return existingReviews;
        }

        ReviewView lastReviewInPage = existingReviews[maxPageSize - 1];
        nextPageToken = lastReviewInPage.Id.ToString();
        return existingReviews.Take(maxPageSize).ToList();
    }
}
