DO
$$
    BEGIN
        CREATE TABLE IF NOT EXISTS supplier_addresses
        (
            supplier_id                  BIGINT
                CONSTRAINT fk_supplier_id
                    REFERENCES suppliers (supplier_id)
                    ON DELETE CASCADE,
            supplier_address_street      VARCHAR(255),
            supplier_address_city        VARCHAR(100),
            supplier_address_postal_code VARCHAR(20),
            supplier_address_country     VARCHAR(100)
        );
    END
$$
