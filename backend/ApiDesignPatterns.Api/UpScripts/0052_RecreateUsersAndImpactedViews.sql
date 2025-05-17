DO
$$
    BEGIN
        DROP VIEW IF EXISTS users_view;

        CREATE OR REPLACE VIEW users_view AS
        SELECT u.user_id,
               CONCAT(u.user_firstname, ' ', u.user_lastname) AS user_fullname,
               u.user_email,
               u.user_created_at
        FROM users u;

        DROP VIEW IF EXISTS inventory_view;
        CREATE OR REPLACE VIEW inventory_view AS
        SELECT i.inventory_id,
               i.user_id,
               i.product_id,
               i.inventory_quantity,
               i.inventory_restock_date
        FROM inventory i;

        DROP VIEW IF EXISTS phone_numbers_view;
        CREATE OR REPLACE VIEW phone_numbers_view AS
        SELECT p.phone_number_id,
               p.user_id,
               CONCAT(p.phone_number_country_code, ' ', p.phone_number_area_code, ' ',
                      p.phone_number_digits) AS phone_number
        FROM phone_numbers p;

        DROP VIEW IF EXISTS addresses_view;
        CREATE OR REPLACE VIEW addresses_view AS
        SELECT address_id,
               user_id,
               CONCAT
               (address_street, ', ',
                address_city, ', ',
                address_postal_code, ', ',
                address_country)
                   AS full_address
        FROM addresses;
    END
$$;
