// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.InfrastructureLayer.Queries;

public static class ProductQueries
{
    public const string GetProduct = """
                                     SELECT
                                         product_id AS Id,
                                         product_name AS Name,
                                         product_category AS Category,
                                         product_dimensions_length_cm AS Length,
                                         product_dimensions_width_cm AS Width,
                                         product_dimensions_height_cm AS Height,
                                         product_base_price AS BasePrice,
                                         product_discount_percentage AS DiscountPercentage,
                                         product_tax_rate AS TaxRate
                                     FROM products
                                     WHERE product_id = @Id;
                                     """;

    public const string CreateProduct = """
                                        INSERT INTO products (
                                        product_name,
                                        product_dimensions_length_cm,
                                        product_dimensions_width_cm,
                                        product_dimensions_height_cm,
                                        product_category,
                                        product_base_price,
                                        product_discount_percentage,
                                        product_tax_rate
                                        )
                                        VALUES (
                                            @Name,
                                            @Length,
                                            @Width,
                                            @Height,
                                            @Category,
                                            @BasePrice,
                                            @DiscountPercentage,
                                            @TaxRate
                                            );
                                        """;


    public const string DeleteProduct = """
                                        DELETE FROM products
                                        WHERE product_id = @Id;
                                        """;

    public const string UpdateProduct = """
                                        UPDATE products
                                        SET
                                        product_name = @Name,
                                        product_dimensions_length_cm = @Length,
                                        product_dimensions_width_cm = @Width,
                                        product_dimensions_height_cm = @Height,
                                        product_category = @Category,
                                        product_base_price = @BasePrice,
                                        product_discount_percentage = @DiscountPercentage,
                                        product_tax_rate = @TaxRate
                                        WHERE product_id = @Id;
                                        """;
}
