DO
$$
    BEGIN
        CREATE OR REPLACE VIEW products_pricing_view AS
        SELECT p.product_id,
               p.product_base_price,
               p.product_discount_percentage,
               p.product_tax_rate
        FROM products p;
    END;
$$