DO
$$
    BEGIN
        -- Check and add product_base_price column if it doesn't exist
        IF NOT EXISTS (SELECT 1
                       FROM information_schema.columns
                       WHERE table_name = 'products'
                         AND column_name = 'product_base_price') THEN
            ALTER TABLE products
                ADD COLUMN product_base_price DECIMAL;
        END IF;

        -- Check and add product_discount_percentage column if it doesn't exist
        IF NOT EXISTS (SELECT 1
                       FROM information_schema.columns
                       WHERE table_name = 'products'
                         AND column_name = 'product_discount_percentage') THEN
            ALTER TABLE products
                ADD COLUMN product_discount_percentage DECIMAL;
        END IF;

        -- Check and add product_tax_rate column if it doesn't exist
        IF NOT EXISTS (SELECT 1
                       FROM information_schema.columns
                       WHERE table_name = 'products'
                         AND column_name = 'product_tax_rate') THEN
            ALTER TABLE products
                ADD COLUMN product_tax_rate DECIMAL;
        END IF;
    END
$$;