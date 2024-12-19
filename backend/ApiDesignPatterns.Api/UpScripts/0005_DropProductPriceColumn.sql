DO
$$
    BEGIN
        IF EXISTS (SELECT 1
                   FROM information_schema.columns
                   WHERE table_name = 'products'
                     AND column_name = 'product_price') THEN
            ALTER TABLE products
                DROP COLUMN product_price;
        END IF;
    END
$$;
