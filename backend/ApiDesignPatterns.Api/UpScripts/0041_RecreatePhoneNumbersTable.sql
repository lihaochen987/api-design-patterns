DO
$$
    BEGIN
        DROP TABLE IF EXISTS supplier_phone_numbers CASCADE;

        CREATE TABLE phone_numbers
        (
            phone_number_id           SERIAL PRIMARY KEY,
            supplier_id               BIGINT
                CONSTRAINT fk_supplier_id
                    REFERENCES suppliers (supplier_id)
                    ON DELETE SET NULL,
            phone_number_country_code VARCHAR(10),
            phone_number_area_code    VARCHAR(10),
            phone_number_digits       VARCHAR(50)
        );
    END
$$;
