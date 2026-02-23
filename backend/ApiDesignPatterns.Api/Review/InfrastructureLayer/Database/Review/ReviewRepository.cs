// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Review.InfrastructureLayer.Database.Review;

public class ReviewRepository(IDbConnection dbConnection) : IReviewRepository
{
    public async Task<DomainModels.Review?> GetReviewAsync(long id)
    {
        var row = await dbConnection.QuerySingleOrDefaultAsync<ReviewRow>(
            ReviewQueries.GetReview, new { Id = id });

        return row is null
            ? null
            : new DomainModels.Review(
                row.Id, row.ProductId, row.Rating, row.Text, row.CreatedAt, row.UpdatedAt);
    }

    public async Task CreateReviewAsync(DomainModels.Review review)
    {
        await dbConnection.ExecuteAsync(ReviewQueries.CreateReview,
            new
            {
                review.ProductId,
                review.Rating,
                review.Text,
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
                review.Rating,
                review.Text,
                review.CreatedAt,
                review.UpdatedAt
            });
    }
}
