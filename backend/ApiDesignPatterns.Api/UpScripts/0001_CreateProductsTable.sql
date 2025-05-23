-- Check if the table 'products' exists before creating it
DO
$$
    BEGIN
        IF to_regclass('public.products') IS NULL THEN
            CREATE TABLE public.products
            (
                product_id                   BIGINT GENERATED BY DEFAULT AS IDENTITY
                    CONSTRAINT "PK_Products" PRIMARY KEY,
                product_name                 VARCHAR(100) NOT NULL,
                product_price                NUMERIC      NOT NULL,
                product_category             TEXT         NOT NULL,
                product_dimensions_length_cm NUMERIC      NOT NULL,
                product_dimensions_width_cm  NUMERIC      NOT NULL,
                product_dimensions_height_cm NUMERIC      NOT NULL
            );
        END IF;
    END
$$;
