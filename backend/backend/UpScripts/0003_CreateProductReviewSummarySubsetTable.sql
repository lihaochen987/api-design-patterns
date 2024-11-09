-- Check if the table 'product_review_summaries' exists before creating it
DO
$$
    BEGIN
        IF to_regclass('public.product_review_summaries') IS NULL THEN
            CREATE TABLE product_review_summaries
            (
                product_id          integer
                    CONSTRAINT product_id
                        REFERENCES products,
                product_one_star    integer,
                product_two_stars   integer,
                product_three_stars integer,
                product_four_stars  integer,
                product_five_stars  integer
            );

            ALTER TABLE product_review_summaries
                OWNER TO myusername;

        END IF;
    END
$$;