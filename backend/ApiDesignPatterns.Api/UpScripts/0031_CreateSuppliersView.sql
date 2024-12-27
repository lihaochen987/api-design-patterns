DO
$$
    BEGIN
        CREATE OR REPLACE VIEW suppliers_view AS
        SELECT s.supplier_id,
               CONCAT(s.supplier_firstname, ' ', s.supplier_lastname) AS supplier_fullname,
               s.supplier_email,
               s.supplier_created_at,
               sa.supplier_address_street,
               sa.supplier_address_city,
               sa.supplier_address_postal_code,
               sa.supplier_address_country,
               CONCAT(spn.supplier_phone_country_code, ' ', spn.supplier_phone_area_code, ' ',
                      spn.supplier_phone_number)                      AS supplier_phone_number
        FROM suppliers s
                 LEFT JOIN public.supplier_phone_numbers spn ON spn.supplier_id = s.supplier_id
                 LEFT JOIN public.supplier_addresses sa ON sa.supplier_id = s.supplier_id;
    END;
$$
