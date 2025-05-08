DO
$$
    BEGIN
        CREATE OR REPLACE VIEW suppliers_view AS
        SELECT s.supplier_id,
               CONCAT(s.supplier_firstname, ' ', s.supplier_lastname) AS supplier_fullname,
               s.supplier_email,
               s.supplier_created_at,
               CONCAT(spn.phone_number_country_code, ' ', spn.phone_number_area_code, ' ',
                      spn.phone_number_digits)                        AS supplier_phone_number,
               CONCAT_WS(', ',
                         sa.supplier_address_street,
                         sa.supplier_address_city,
                         sa.supplier_address_postal_code,
                         sa.supplier_address_country
               )                                                      AS full_address
        FROM suppliers s
                 LEFT JOIN phone_numbers spn ON spn.supplier_id = s.supplier_id
                 LEFT JOIN supplier_addresses sa ON sa.supplier_id = s.supplier_id;
    END
$$;
