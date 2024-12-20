// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Linq.Expressions;
using backend.Review.DomainModels.Views;
using backend.Review.InfrastructureLayer.Database;
using backend.Shared;
using backend.Shared.CelSpec;
using Microsoft.EntityFrameworkCore;

namespace backend.Review.InfrastructureLayer;

public class ReviewViewRepository(
    ReviewDbContext context)
    : IReviewViewRepository
{
    public async Task<ReviewView?> GetReviewView(long id) =>
        await context.Set<ReviewView>()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<(List<ReviewView>, string?)> ListReviewsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent)
    {
        IQueryable<ReviewView> query = context.Set<ReviewView>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(parent) && long.TryParse(parent, out long parentId))
        {
            query = query.Where(p => p.ProductId == parentId);
        }

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

        List<ReviewView> paginatedReviews = Paginate(reviews, maxPageSize, out string? nextPageToken);

        return (paginatedReviews, nextPageToken);
    }

    private static Expression<Func<ReviewView, bool>> BuildFilterExpression(string filter)
    {
        CelParser<ReviewView> parser = new(new TypeParser());
        List<CelToken> tokens = parser.Tokenize(filter);
        return parser.ParseFilter(tokens);
    }

    private static List<ReviewView> Paginate(
        List<ReviewView> existingItems,
        int maxPageSize,
        out string? nextPageToken)
    {
        if (existingItems.Count <= maxPageSize)
        {
            nextPageToken = null;
            return existingItems;
        }

        ReviewView lastItemInPage = existingItems[maxPageSize - 1];
        nextPageToken = lastItemInPage.Id.ToString();
        return existingItems.Take(maxPageSize).ToList();
    }
}
