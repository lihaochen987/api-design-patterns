DO
$$
    BEGIN
        INSERT INTO suppliers (supplier_firstname,
                               supplier_lastname,
                               supplier_email,
                               supplier_created_at)
        VALUES ('John', 'Doe', 'john.doe@example.com', CURRENT_TIMESTAMP),
               ('Jane', 'Smith', 'jane.smith@example.com', CURRENT_TIMESTAMP),
               ('Carlos', 'Martinez', 'carlos.martinez@example.com', CURRENT_TIMESTAMP),
               ('Aiko', 'Tanaka', 'aiko.tanaka@example.jp', CURRENT_TIMESTAMP),
               ('Fatima', 'Ali', 'fatima.ali@example.ae', CURRENT_TIMESTAMP);

        INSERT INTO supplier_addresses (supplier_id,
                                        supplier_address_street,
                                        supplier_address_city,
                                        supplier_address_postal_code,
                                        supplier_address_country)
        VALUES (1, '123 Main St', 'Springfield', '62701', 'USA'),
               (2, '45 Queen St', 'London', 'SW1A 1AA', 'United Kingdom'),
               (3, 'Calle Gran Via 23', 'Madrid', '28013', 'Spain'),
               (4, '5-1-2 Ginza', 'Tokyo', '104-0061', 'Japan'),
               (5, 'Sheikh Zayed Rd', 'Dubai', '00000', 'United Arab Emirates');

        INSERT INTO supplier_phone_numbers (supplier_id,
                                            supplier_phone_country_code,
                                            supplier_phone_area_code,
                                            supplier_phone_number)
        VALUES (1, '+1', '123', '5556789'),
               (2, '+44', '20', '79460000'),
               (3, '+34', '91', '1234567'),
               (4, '+81', '03', '987654321'),
               (5, '+971', '04', '1234567890');
    END
$$;
