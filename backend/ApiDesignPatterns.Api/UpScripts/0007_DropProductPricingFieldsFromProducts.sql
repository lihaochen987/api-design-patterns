DO
$$
    BEGIN
        IF EXISTS (SELECT 1
                   FROM information_schema.columns
                   WHERE table_name = 'products'
                     AND column_name = 'product_base_price') THEN
            ALTER TABLE products
                DROP COLUMN product_base_price;
        END IF;

        IF EXISTS (SELECT 1
                   FROM information_schema.columns
                   WHERE table_name = 'products'
                     AND column_name = 'product_discount_percentage') THEN
            ALTER TABLE products
                DROP COLUMN product_discount_percentage;
        END IF;

        IF EXISTS (SELECT 1
                   FROM information_schema.columns
                   WHERE table_name = 'products'
                     AND column_name = 'product_tax_rate') THEN
            ALTER TABLE products
                DROP COLUMN product_tax_rate;
        END IF;
    END
$$;
