DO
$$
    BEGIN
        ALTER TABLE products
            ADD COLUMN product_base_price          NUMERIC NOT NULL DEFAULT 0
                CONSTRAINT chk_product_base_price
                    CHECK (product_base_price >= 0),
            ADD COLUMN product_discount_percentage NUMERIC NOT NULL DEFAULT 0
                CONSTRAINT chk_product_discount_percentage
                    CHECK (product_discount_percentage >= 0 AND product_discount_percentage <= 100),
            ADD COLUMN product_tax_rate            NUMERIC NOT NULL DEFAULT 0
                CONSTRAINT chk_product_tax_rate
                    CHECK (product_tax_rate >= 0 AND product_tax_rate <= 100);

        UPDATE products
        SET product_base_price          = pp.product_base_price,
            product_discount_percentage = pp.product_discount_percentage,
            product_tax_rate            = pp.product_tax_rate
        FROM product_pricing pp
        WHERE products.product_id = pp.product_id;

        DROP TRIGGER IF EXISTS prevent_delete_trigger ON product_pricing;
        DROP TRIGGER IF EXISTS prevent_product_pricing_delete_trigger ON product_pricing;

        DROP TABLE IF EXISTS product_pricing;

        ALTER TABLE products
            DROP CONSTRAINT IF EXISTS chk_product_base_price,
            DROP CONSTRAINT IF EXISTS chk_product_discount_percentage,
            DROP CONSTRAINT IF EXISTS chk_product_tax_rate;

        ALTER TABLE products
            ADD CONSTRAINT chk_product_base_price
                CHECK (product_base_price >= 0),
            ADD CONSTRAINT chk_product_discount_percentage
                CHECK (product_discount_percentage >= 0 AND product_discount_percentage <= 100),
            ADD CONSTRAINT chk_product_tax_rate
                CHECK (product_tax_rate >= 0 AND product_tax_rate <= 100);
    END;
$$