DO
$$
    BEGIN
        CREATE OR REPLACE VIEW phone_numbers_view AS
        SELECT p.phone_number_id,
               p.supplier_id,
               CONCAT(p.phone_number_country_code, ' ', p.phone_number_area_code, ' ',
                      p.phone_number_digits) AS phone_number
        FROM phone_numbers p;
    END
$$;
