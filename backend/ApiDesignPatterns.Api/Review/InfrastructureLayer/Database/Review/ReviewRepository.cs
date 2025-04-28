// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Review.InfrastructureLayer.Database.Review;

public class ReviewRepository(
    IDbConnection dbConnection)
    : IReviewRepository
{
    public async Task<DomainModels.Review?> GetReviewAsync(long id)
    {
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.Review>(ReviewQueries.GetReview,
            new { Id = id });
    }

    public async Task CreateReviewAsync(DomainModels.Review review)
    {
        await dbConnection.ExecuteAsync(ReviewQueries.CreateReview,
            new
            {
                review.ProductId,
                Rating = review.Rating.Value,
                Text = review.Text.Value,
                review.CreatedAt,
                review.UpdatedAt
            });
    }

    public async Task DeleteReviewAsync(long id)
    {
        await dbConnection.ExecuteAsync(ReviewQueries.DeleteReview, new { Id = id });
    }

    public async Task UpdateReviewAsync(DomainModels.Review review)
    {
        await dbConnection.ExecuteAsync(ReviewQueries.UpdateReview,
            new
            {
                review.Id,
                review.ProductId,
                Rating = review.Rating.Value,
                Text = review.Text.Value,
                review.CreatedAt,
                review.UpdatedAt
            });
    }
}
