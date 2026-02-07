DO
$$
    BEGIN
        ALTER TABLE reviews
            ADD COLUMN review_public_id UUID DEFAULT gen_random_uuid();

        UPDATE reviews
        SET review_public_id = gen_random_uuid()
        WHERE review_public_id IS NULL;

        ALTER TABLE reviews
            ALTER COLUMN review_public_id SET NOT NULL;

        CREATE UNIQUE INDEX IX_reviews_public_id ON reviews (review_public_id);
    END;
$$
