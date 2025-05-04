DO
$$
    BEGIN
        INSERT INTO product_feeders (product_id, product_feeders_type_id, product_feeders_material_id,
                                     product_feeders_capacity_liters, product_feeders_dishwasher_safe,
                                     product_feeders_battery_operated, product_feeders_battery_type,
                                     product_feeders_cleaning_instructions)
        VALUES
            (12, 4, 3, 0.8, true, false, NULL,
             'Dishwasher safe, gentle cycle recommended'),

            (13, 1, 2, 3.5, false, true, 'AA Batteries',
             'Wipe with damp cloth, removable parts dishwasher safe'),

            (24, 3, 1, 4.2, false, true, 'Rechargeable Lithium-ion',
             'Clean with damp cloth only, removable bowl is dishwasher safe'),

            (25, 2, 1, 3.0, true, false, NULL,
             'Rinse thoroughly after each use, dishwasher safe'),

            (26, 4, 2, 1.2, true, false, NULL,
             'Dishwasher safe, handwash for longevity'),

            (27, 4, 4, 0.7, true, false, NULL,
             'Dishwasher and microwave safe, collapsible for storage'),

            (28, 4, 3, 0.5, true, false, NULL,
             'Dishwasher safe, avoid abrasive cleaners'),

            (29, 2, 1, 3.8, false, false, NULL,
             'Hand wash with warm soapy water, air dry'),

            (30, 4, 2, 1.5, true, false, NULL,
             'Wipe frame with damp cloth, bowls are dishwasher safe'),

            (31, 2, 1, 4.5, true, false, NULL,
             'Rinse reservoir weekly, clean with vinegar monthly'),

            (32, 4, 3, 0.9, true, false, NULL,
             'Dishwasher and microwave safe, handle with care'),

            (33, 3, 1, 3.2, false, true, 'D Batteries',
             'Wipe electronics with dry cloth, containers are dishwasher safe'),

            (34, 4, 2, 1.0, true, false, NULL,
             'Dishwasher safe, top rack recommended'),

            (35, 4, 2, 1.2, true, false, NULL,
             'Hand wash with mild soap, rinse thoroughly'),

            (36, 3, 1, 2.0, false, true, 'Rechargeable Lithium-ion',
             'Wipe with dry cloth, empty treat compartment weekly'),

            (37, 2, 3, 2.8, true, false, NULL,
             'Clean filter monthly, dishwasher safe components')
        ON CONFLICT (product_id) DO NOTHING;
    END
$$;
