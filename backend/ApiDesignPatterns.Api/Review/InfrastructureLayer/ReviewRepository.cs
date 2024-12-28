// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Data;
using Dapper;

namespace backend.Review.InfrastructureLayer;

public class ReviewRepository(IDbConnection dbConnection) : IReviewRepository
{
    public async Task<DomainModels.Review?> GetReviewAsync(long id)
    {
        const string query = """

                                             SELECT
                                                 review_id AS Id,
                                                 product_id AS ProductId,
                                                 review_rating AS Rating,
                                                 review_text AS Text,
                                                 review_created_at AS CreatedAt,
                                                 review_updated_at AS UpdatedAt
                                             FROM reviews
                                             WHERE review_id = @Id
                             """;
        return await dbConnection.QuerySingleOrDefaultAsync<DomainModels.Review>(query, new { Id = id });
    }

    public async Task CreateReviewAsync(DomainModels.Review review)
    {
        const string query = """

                                             INSERT INTO reviews (product_id, review_rating, review_text, review_created_at, review_updated_at)
                                             VALUES (@ProductId, @Rating, @Text, @CreatedAt, @UpdatedAt)
                             """;

        await dbConnection.ExecuteAsync(query, new
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
        const string query = "DELETE FROM reviews WHERE review_id = @Id";
        await dbConnection.ExecuteAsync(query, new { Id = id });
    }

    public async Task UpdateReviewAsync(DomainModels.Review review)
    {
        const string query = """

                                             UPDATE reviews
                                             SET
                                                 product_id = @ProductId,
                                                 review_rating = @Rating,
                                                 review_text = @Text,
                                                 review_created_at = @CreatedAt,
                                                 review_updated_at = @UpdatedAt
                                             WHERE review_id = @Id
                             """;
        await dbConnection.ExecuteAsync(query, new
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
