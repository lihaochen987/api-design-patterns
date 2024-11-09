-- Add product_last_reviewed_date and product_last_reviewed_time columns to products table
-- if they do not already exist

DO $$
    BEGIN
        -- Check and add product_last_reviewed_date column if it doesn't exist
        IF NOT EXISTS (
            SELECT 1
            FROM information_schema.columns
            WHERE table_name = 'products'
              AND column_name = 'product_last_reviewed_date'
        ) THEN
            ALTER TABLE products
                ADD COLUMN product_last_reviewed_date DATE;
        END IF;

        -- Check and add product_last_reviewed_time column if it doesn't exist
        IF NOT EXISTS (
            SELECT 1
            FROM information_schema.columns
            WHERE table_name = 'products'
              AND column_name = 'product_last_reviewed_time'
        ) THEN
            ALTER TABLE products
                ADD COLUMN product_last_reviewed_time TIME;
        END IF;
    END
$$;