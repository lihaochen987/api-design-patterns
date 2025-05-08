DO
$$
    BEGIN
        DROP TABLE IF EXISTS supplier_addresses;

        CREATE TABLE IF NOT EXISTS addresses
        (
            address_id                   SERIAL PRIMARY KEY,
            supplier_id                  BIGINT
                CONSTRAINT fk_supplier_id
                    REFERENCES suppliers (supplier_id)
                    ON DELETE SET NULL,
            address_street      VARCHAR(255),
            address_city        VARCHAR(100),
            address_postal_code VARCHAR(20),
            address_country     VARCHAR(100)
        );
    END
$$
