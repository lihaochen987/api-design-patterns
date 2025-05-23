DO
$$
    BEGIN
        IF to_regclass('public.reviews') IS NULL THEN
            CREATE TABLE public.reviews
            (
                review_id         BIGINT GENERATED BY DEFAULT AS IDENTITY
                    CONSTRAINT "PK_Reviews" PRIMARY KEY,
                product_id        BIGINT                                NOT NULL
                    CONSTRAINT "FK_Reviews_Products" REFERENCES public.products (product_id)
                        ON DELETE CASCADE,
                review_rating     NUMERIC(2, 1)                         NOT NULL CHECK (review_rating BETWEEN 0 AND 5),
                review_text       TEXT,
                review_created_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP NOT NULL,
                review_updated_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP NOT NULL
            );
        END IF;
    END
$$;
