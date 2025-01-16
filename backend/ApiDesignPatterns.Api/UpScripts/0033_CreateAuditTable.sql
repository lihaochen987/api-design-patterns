DO
$$
    BEGIN
        CREATE TABLE IF NOT EXISTS audit
        (
            id              SERIAL PRIMARY KEY,
            audit_timestamp TIMESTAMP DEFAULT NOW(),
            operation       VARCHAR(100) NOT NULL,
            data            JSONB        NOT NULL
        );
    END;
$$;
