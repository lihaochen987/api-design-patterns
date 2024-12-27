DO
$$
    BEGIN
        ALTER TABLE reviews
            ALTER COLUMN review_updated_at DROP NOT NULL;
        COMMIT;

        ALTER TABLE reviews
            ALTER COLUMN review_updated_at DROP DEFAULT;
        COMMIT;

        UPDATE reviews
        SET review_updated_at = NULL;
    END
$$
