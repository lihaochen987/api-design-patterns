DO
$$
    BEGIN
        ALTER TABLE products
            DROP CONSTRAINT IF EXISTS chk_dimensions_length,
            DROP CONSTRAINT IF EXISTS chk_dimensions_width,
            DROP CONSTRAINT IF EXISTS chk_dimensions_height,
            DROP CONSTRAINT IF EXISTS chk_dimensions_volume;


        ALTER TABLE products
            ADD CONSTRAINT chk_dimensions_length
                CHECK (product_dimensions_length_cm > 0 AND product_dimensions_length_cm <= 100),
            ADD CONSTRAINT chk_dimensions_width
                CHECK (product_dimensions_width_cm > 0 AND product_dimensions_width_cm <= 50),
            ADD CONSTRAINT chk_dimensions_height
                CHECK (product_dimensions_width_cm > 0 AND product_dimensions_width_cm <= 50),
            ADD CONSTRAINT chk_dimensions_volume
                CHECK (products.product_dimensions_length_cm * product_dimensions_width_cm *
                       product_dimensions_height_cm <=
                       110000);
    END
$$
