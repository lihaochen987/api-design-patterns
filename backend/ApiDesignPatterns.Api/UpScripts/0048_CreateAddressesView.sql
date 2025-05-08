DO
$$
    BEGIN
        CREATE OR REPLACE VIEW addresses_view AS
        SELECT address_id,
               supplier_id,
               CONCAT
               (address_street, ', ',
                address_city, ', ',
                address_postal_code, ', ',
                address_country)
                   AS full_address
        FROM addresses;

    END
$$;
