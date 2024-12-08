// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels.Views;
using backend.Review.InfrastructureLayer;
using backend.Review.ReviewControllers;

namespace backend.Review.ApplicationLayer;

public class ReviewViewApplicationService(IReviewViewRepository repository) : IReviewViewApplicationService
{
    public async Task<ReviewView?> GetReviewView(long id)
    {
        // Prepare
        ReviewView? review = await repository.GetReviewView(id);

        // Apply
        return review;
    }

    public async Task<(List<ReviewView>, string?)> ListProductsAsync(ListReviewsRequest request)
    {
        // Prepare
        (List<ReviewView> reviews, string? nextPageToken) = await repository.ListReviewsAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);

        // Apply
        return (reviews, nextPageToken);
    }
}
