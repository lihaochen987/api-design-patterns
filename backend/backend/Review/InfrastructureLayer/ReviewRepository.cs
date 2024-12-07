// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database;

namespace backend.Review.InfrastructureLayer;

public class ReviewRepository(ReviewDbContext context) : IReviewRepository
{
    public async Task<DomainModels.Review?> GetReviewAsync(long id) => await context.Reviews.FindAsync(id);

    public async Task CreateReviewAsync(DomainModels.Review review)
    {
        context.Reviews.Add(review);
        await context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(DomainModels.Review review)
    {
        context.Reviews.Remove(review);
        await context.SaveChangesAsync();
    }
}
