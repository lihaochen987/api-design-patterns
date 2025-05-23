DO
$$
    BEGIN
        CREATE OR REPLACE VIEW products_view AS
        SELECT p.product_id,
               p.product_name,
               p.product_dimensions_length_cm,
               p.product_dimensions_width_cm,
               p.product_dimensions_height_cm,
               c.product_category_name,
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
