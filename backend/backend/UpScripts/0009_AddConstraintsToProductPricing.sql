DO
$$
    BEGIN
        -- Add constraint for product_base_price if it doesn't exist
        IF
            NOT EXISTS (SELECT 1
                        FROM information_schema.table_constraints
                        WHERE table_name = 'product_pricing'
                          AND constraint_name = 'chk_product_base_price') THEN
            ALTER TABLE product_pricing
                ADD CONSTRAINT chk_product_base_price
                    CHECK (product_base_price >= 0);
        END IF;
        -- Add constraint for product_discount_percentage if it doesn't exist
        IF
            NOT EXISTS (SELECT 1
                        FROM information_schema.table_constraints
                        WHERE table_name = 'product_pricing'
                          AND constraint_name = 'chk_product_discount_percentage') THEN
            ALTER TABLE product_pricing
                ADD CONSTRAINT chk_product_discount_percentage
                    CHECK (product_discount_percentage >= 0 AND product_discount_percentage <= 100);
        END IF;

        -- Add constraint for product_tax_rate if it doesn't exist
        IF NOT EXISTS (SELECT 1
                       FROM information_schema.table_constraints
                       WHERE table_name = 'product_pricing'
                         AND constraint_name = 'chk_product_tax_rate') THEN
            ALTER TABLE product_pricing
                ADD CONSTRAINT chk_product_tax_rate
                    CHECK (product_tax_rate >= 0 AND product_tax_rate <= 100);
        END IF;

    END
$$;