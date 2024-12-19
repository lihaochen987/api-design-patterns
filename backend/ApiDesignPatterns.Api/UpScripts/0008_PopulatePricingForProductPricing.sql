DO
$$
    BEGIN
        INSERT INTO product_pricing (product_id, product_base_price, product_discount_percentage, product_tax_rate)
        VALUES (1, 50.00, 5.0, 7.5),
               (2, 35.00, 10.0, 7.5),
               (3, 10.00, 0.0, 7.5),
               (4, 15.00, 3.0, 0.0),
               (5, 8.00, 2.0, 0.0),
               (6, 12.00, 5.0, 0.0),
               (7, 20.00, 0.0, 0.0),
               (8, 10.00, 0.0, 5.0),
               (9, 7.00, 0.0, 5.0),
               (10, 80.00, 15.0, 0.0),
               (11, 9.00, 2.0, 0.0),
               (12, 6.00, 0.0, 0.0),
               (13, 45.00, 5.0, 0.0),
               (14, 65.00, 10.0, 5.0),
               (15, 18.00, 3.0, 0.0),
               (16, 5.00, 0.0, 0.0),
               (17, 25.00, 5.0, 7.5),
               (18, 15.00, 0.0, 7.5),
               (19, 35.00, 10.0, 0.0),
               (20, 25.00, 5.0, 0.0),
               (21, 50.00, 8.0, 7.5),
               (22, 35.00, 7.5, 7.5),
               (23, 10.00, 0.0, 7.5)
        ON CONFLICT (product_id) DO UPDATE
            SET product_base_price          = EXCLUDED.product_base_price,
                product_discount_percentage = EXCLUDED.product_discount_percentage,
                product_tax_rate            = EXCLUDED.product_tax_rate;
    END;
$$;
