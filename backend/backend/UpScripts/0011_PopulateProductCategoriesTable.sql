DO
$$
    BEGIN
        INSERT INTO product_categories (product_category_id, product_category_name)
        VALUES (1, 'PetFood'),
               (2, 'Toys'),
               (3, 'CollarsAndLeashes'),
               (4, 'GroomingAndHygiene'),
               (5, 'Beds'),
               (6, 'Feeders'),
               (7, 'TravelAccessories'),
               (8, 'Clothing')
        ON CONFLICT (product_category_id) DO NOTHING;
    END;
$$