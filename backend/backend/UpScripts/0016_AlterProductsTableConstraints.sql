ALTER TABLE products
    DROP CONSTRAINT IF EXISTS chk_dimensions_length,
    DROP CONSTRAINT IF EXISTS chk_dimensions_width,
    DROP CONSTRAINT IF EXISTS chk_dimensions_height,
    DROP CONSTRAINT IF EXISTS chk_dimensions_volume;


ALTER TABLE products
    ADD CONSTRAINT chk_dimensions_length
        CHECK (product_dimensions_length > 0 AND product_dimensions_length <= 100),
    ADD CONSTRAINT chk_dimensions_width
        CHECK (product_dimensions_width > 0 AND product_dimensions_width <= 50),
    ADD CONSTRAINT chk_dimensions_height
        CHECK (product_dimensions_width > 0 AND product_dimensions_width <= 50),
    ADD CONSTRAINT chk_dimensions_volume
        CHECK (products.product_dimensions_length * product_dimensions_width * product_dimensions_height <= 110000);