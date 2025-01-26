// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer;
using backend.Review.InfrastructureLayer.Database.Review;
using backend.Review.ReviewControllers;
using backend.Review.Services;

namespace backend.Review.ApplicationLayer;

public class ReviewApplicationService(
    IReviewRepository repository,
    ReviewFieldMaskConfiguration maskConfiguration)
    : IReviewApplicationService
{
    public async Task CreateReviewAsync(DomainModels.Review review)
    {
        await repository.CreateReviewAsync(review);
    }

    public async Task DeleteReviewAsync(long id) => await repository.DeleteReviewAsync(id);

    public async Task ReplaceReviewAsync(DomainModels.Review review)
    {
        await repository.UpdateReviewAsync(review);
    }

    public async Task UpdateReviewAsync(UpdateReviewRequest request, DomainModels.Review review)
    {
        (long productId, decimal rating, string text) =
            maskConfiguration.GetUpdatedReviewValues(request, review);
        review.ProductId = productId;
        review.Rating = rating;
        review.Text = text;
        review.UpdatedAt = DateTimeOffset.UtcNow;
        await repository.UpdateReviewAsync(review);
    }
}
