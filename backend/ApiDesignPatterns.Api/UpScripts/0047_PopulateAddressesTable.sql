DO
$$
    BEGIN
        INSERT INTO addresses (supplier_id,
                               address_street,
                               address_city,
                               address_postal_code,
                               address_country)
        VALUES (1, '123 Main St', 'Springfield', '62701', 'USA'),
               (2, '45 Queen St', 'London', 'SW1A 1AA', 'United Kingdom'),
               (3, 'Calle Gran Via 23', 'Madrid', '28013', 'Spain'),
               (4, '5-1-2 Ginza', 'Tokyo', '104-0061', 'Japan'),
               (5, 'Sheikh Zayed Rd', 'Dubai', '00000', 'United Arab Emirates');
    END
$$;
