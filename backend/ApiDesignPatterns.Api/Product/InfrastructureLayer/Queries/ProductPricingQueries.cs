// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.InfrastructureLayer.Queries;

public static class ProductPricingQueries
{
    public const string GetProductPricing = """
                                        SELECT
                                            product_id AS ProductId,
                                            review_rating AS Rating,
                                            review_text AS Text,
                                            review_created_at AS CreatedAt,
                                            review_updated_at AS UpdatedAt
                                        FROM reviews_view
                                        WHERE review_id = @Id;
                                        """;
}
