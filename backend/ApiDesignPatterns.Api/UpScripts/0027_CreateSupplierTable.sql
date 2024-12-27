DO
$$
    BEGIN
        CREATE TABLE IF NOT EXISTS suppliers
        (
            supplier_id                 SERIAL PRIMARY KEY,
            supplier_firstname          VARCHAR(255) NOT NULL,
            supplier_lastname           VARCHAR(255) NOT NULL,
            supplier_email              VARCHAR(255),
            supplier_created_at         TIMESTAMP DEFAULT NOW()
        );
    END
$$
