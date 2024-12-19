-- Check if the table 'products' exists before creating it
DO
$$
    BEGIN
        IF to_regclass('public.product_pricing') IS NULL THEN
            CREATE TABLE public.product_pricing
            (
                product_id                  BIGINT  NOT NULL
                    CONSTRAINT "FK_ProductPricing_Products" REFERENCES public.products (product_id) ON DELETE CASCADE,
                product_base_price          NUMERIC NOT NULL,
                product_discount_percentage NUMERIC NOT NULL,
                product_tax_rate            NUMERIC NOT NULL,
                CONSTRAINT unique_product_id UNIQUE (product_id)
            );
        END IF;

        ALTER TABLE product_pricing
            OWNER TO myusername;
    END
$$;