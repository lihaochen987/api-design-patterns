DO
$$
    BEGIN
        ALTER TABLE products
            ADD COLUMN product_category_id BIGINT;

        UPDATE products
        SET product_category_id = (
            SELECT product_category_id
            FROM product_categories
            WHERE product_categories.product_category_name = products.product_category
        );

    END;
$$
