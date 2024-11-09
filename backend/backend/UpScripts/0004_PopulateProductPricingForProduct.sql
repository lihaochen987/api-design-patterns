DO
$$
    BEGIN
        -- Update only product_base_price, product_discount_percentage, and product_tax_rate for specific products

        UPDATE products
        SET product_base_price          = 50.00,
            product_discount_percentage = 5.0,
            product_tax_rate            = 7.5
        WHERE product_id = 1
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 35.00,
            product_discount_percentage = 10.0,
            product_tax_rate            = 7.5
        WHERE product_id = 2
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 10.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 7.5
        WHERE product_id = 3
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 15.00,
            product_discount_percentage = 3.0,
            product_tax_rate            = 0.0
        WHERE product_id = 4
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 8.00,
            product_discount_percentage = 2.0,
            product_tax_rate            = 0.0
        WHERE product_id = 5
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 12.00,
            product_discount_percentage = 5.0,
            product_tax_rate            = 0.0
        WHERE product_id = 6
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 20.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 0.0
        WHERE product_id = 7
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 10.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 5.0
        WHERE product_id = 8
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 7.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 5.0
        WHERE product_id = 9
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 80.00,
            product_discount_percentage = 15.0,
            product_tax_rate            = 0.0
        WHERE product_id = 10
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 9.00,
            product_discount_percentage = 2.0,
            product_tax_rate            = 0.0
        WHERE product_id = 11
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 6.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 0.0
        WHERE product_id = 12
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 45.00,
            product_discount_percentage = 5.0,
            product_tax_rate            = 0.0
        WHERE product_id = 13
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 65.00,
            product_discount_percentage = 10.0,
            product_tax_rate            = 5.0
        WHERE product_id = 14
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 18.00,
            product_discount_percentage = 3.0,
            product_tax_rate            = 0.0
        WHERE product_id = 15
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 5.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 0.0
        WHERE product_id = 16
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 25.00,
            product_discount_percentage = 5.0,
            product_tax_rate            = 7.5
        WHERE product_id = 17
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 15.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 7.5
        WHERE product_id = 18
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 35.00,
            product_discount_percentage = 10.0,
            product_tax_rate            = 0.0
        WHERE product_id = 19
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 25.00,
            product_discount_percentage = 5.0,
            product_tax_rate            = 0.0
        WHERE product_id = 20
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 50.00,
            product_discount_percentage = 8.0,
            product_tax_rate            = 7.5
        WHERE product_id = 21
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 35.00,
            product_discount_percentage = 7.5,
            product_tax_rate            = 7.5
        WHERE product_id = 22
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

        UPDATE products
        SET product_base_price          = 10.00,
            product_discount_percentage = 0.0,
            product_tax_rate            = 7.5
        WHERE product_id = 23
          AND products.product_base_price IS NULL
          AND products.product_discount_percentage IS NULL
          AND products.product_tax_rate IS NULL;

    END;
$$;
