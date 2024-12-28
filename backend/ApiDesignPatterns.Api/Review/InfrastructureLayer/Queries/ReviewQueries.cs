// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.InfrastructureLayer.Queries;

public static class ReviewQueries
{
    public const string GetReview = """

                                            SELECT
                                                review_id AS Id,
                                                product_id AS ProductId,
                                                review_rating AS Rating,
                                                review_text AS Text,
                                                review_created_at AS CreatedAt,
                                                review_updated_at AS UpdatedAt
                                            FROM reviews
                                            WHERE review_id = @Id;

                                    """;

    public const string CreateReview = """

                                               INSERT INTO reviews (
                                                   product_id,
                                                   review_rating,
                                                   review_text,
                                                   review_created_at,
                                                   review_updated_at)
                                               VALUES (
                                                   @ProductId,
                                                   @Rating,
                                                   @Text,
                                                   @CreatedAt,
                                                   @UpdatedAt);

                                       """;

    public const string DeleteReview = """

                                               DELETE FROM reviews
                                               WHERE review_id = @Id;

                                       """;

    public const string UpdateReview = """

                                               UPDATE reviews
                                               SET
                                                   product_id = @ProductId,
                                                   review_rating = @Rating,
                                                   review_text = @Text,
                                                   review_created_at = @CreatedAt,
                                                   review_updated_at = @UpdatedAt
                                               WHERE review_id = @Id;

                                       """;
}
