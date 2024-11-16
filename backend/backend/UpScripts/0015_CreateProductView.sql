DO
$$
    BEGIN
        CREATE OR REPLACE VIEW product_view AS
        SELECT p.product_id,
               p.product_name,
               p.product_dimensions_length,
               p.product_dimensions_width,
               p.product_dimensions_height,
               c.product_category_name,
               p.product_base_price,
               p.product_discount_percentage,
               p.product_tax_rate,
               ROUND(
                       (p.product_base_price * (1 - p.product_discount_percentage / 100)) *
                       (1 + p.product_tax_rate / 100),
                       2
               ) AS product_price
        FROM products p
                 LEFT JOIN
             product_categories c
             ON
                 p.product_category = c.product_category_id;

    END;
$$