DO
$$
    BEGIN
        ALTER TABLE products
            DROP COLUMN product_category;

        ALTER TABLE products
            RENAME product_category_id TO product_category;

        ALTER TABLE products
            ADD CONSTRAINT fk_product_category
                FOREIGN KEY (product_category)
                    REFERENCES product_categories (product_category_id)
                    ON DELETE CASCADE;
    END;
$$
