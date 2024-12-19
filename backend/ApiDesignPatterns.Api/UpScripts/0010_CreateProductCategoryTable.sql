DO
$$
    BEGIN
        CREATE TABLE product_categories
        (
            product_category_id   SERIAL PRIMARY KEY,
            product_category_name TEXT NOT NULL
        );
    END
$$
