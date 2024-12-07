DO
$$
    BEGIN
        CREATE OR REPLACE VIEW reviews_view AS
        SELECT r.review_id,
               r.product_id,
               r.review_rating,
               r.review_text,
               r.review_created_at,
               r.review_updated_at
        FROM reviews r;
    END;
$$
