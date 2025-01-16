// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.InfrastructureLayer.Database;

public class ProductQueries
{
    public const string GetProduct = """

                                             SELECT
                                                 product_id AS Id,
                                                 product_name AS Name,
                                                 product_category AS Category,
                                                 product_base_price AS BasePrice,
                                                 product_discount_percentage AS DiscountPercentage,
                                                 product_tax_rate AS TaxRate,
                                                 product_dimensions_length_cm AS Length,
                                                 product_dimensions_width_cm AS Width,
                                                 product_dimensions_height_cm AS Height
                                             FROM products
                                             WHERE product_id = @Id;

                                     """;

    public const string CreateProduct = """

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

    public const string DeleteProduct = """

                                                DELETE FROM reviews
                                                WHERE review_id = @Id;

                                        """;

    public const string UpdateProduct = """

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
