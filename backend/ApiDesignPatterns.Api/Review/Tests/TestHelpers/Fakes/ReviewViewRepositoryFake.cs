// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Review.Tests.TestHelpers.Builders;
using backend.Shared;

namespace backend.Review.Tests.TestHelpers.Fakes;

public class ReviewViewRepositoryFake(
    PaginateService<ReviewView> paginateService)
    : Collection<ReviewView>, IReviewViewRepository
{
    public void AddReviewView(long productId)
    {
        var reviewView = new ReviewViewTestDataBuilder()
            .WithProductId(productId)
            .Build();
        Add(reviewView);
    }

    public void AddReviewView(long productId, long rating)
    {
        var reviewView = new ReviewViewTestDataBuilder()
            .WithProductId(productId)
            .WithRating(rating)
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
        var query = this.AsEnumerable();

        // Parent filter
        if (!string.IsNullOrWhiteSpace(parent) && long.TryParse(parent, out long parentId))
        {
            query = query.Where(r => r.ProductId == parentId);
        }

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenReview))
        {
            query = query.Where(r => r.Id > lastSeenReview);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            if (filter.Contains("Rating >="))
            {
                string value = filter.Split(">=")[1].Trim();
                query = query.Where(s => s.Rating >= decimal.Parse(value));
            }
            else
            {
                throw new ArgumentException();
            }
        }

        var reviews = query
            .OrderBy(r => r.Id)
            .Take(maxPageSize + 1)
            .ToList();

        List<ReviewView> paginatedReviews =
            paginateService.Paginate(reviews, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedReviews, nextPageToken));
    }
}
