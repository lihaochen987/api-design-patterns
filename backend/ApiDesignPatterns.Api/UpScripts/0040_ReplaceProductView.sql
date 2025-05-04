DO
$$
    BEGIN
        CREATE OR REPLACE VIEW products_view AS
        SELECT p.product_id,
               p.product_name,
               p.product_dimensions_length_cm,
               p.product_dimensions_width_cm,
               p.product_dimensions_height_cm,
               c.product_category_name,
               ROUND(
                   (p.product_base_price * (1 - p.product_discount_percentage / 100)) *
                   (1 + p.product_tax_rate / 100),
                   2
               ) AS product_price,
               ppfag.product_pet_food_age_group,
               ppfbs.product_pet_food_breed_size,
               ppf.product_pet_foods_ingredients,
               ppf.product_pet_foods_nutritional_info,
               ppf.product_pet_foods_storage_instructions,
               ppf.product_pet_foods_weight_kg,
               pgah.product_grooming_and_hygiene_is_natural,
               pgah.product_grooming_and_hygiene_is_hypoallergenic,
               pgah.product_grooming_and_hygiene_usage_instructions,
               pgah.product_grooming_and_hygiene_is_cruelty_free,
               pgah.product_grooming_and_hygiene_safety_warnings,
               pft.product_feeder_type,
               pfm.product_feeder_material,
               pf.product_feeders_capacity_liters,
               pf.product_feeders_dishwasher_safe,
               pf.product_feeders_battery_operated,
               pf.product_feeders_battery_type,
               pf.product_feeders_cleaning_instructions
        FROM products p
                 LEFT JOIN
             product_categories c
             ON
                 p.product_category = c.product_category_id
                 LEFT JOIN product_pet_foods ppf ON p.product_id = ppf.product_id
                 LEFT JOIN product_pet_food_age_groups ppfag
                           ON ppf.product_pet_foods_age_group_id = ppfag.product_pet_food_age_groups_id
                 LEFT JOIN product_pet_food_breed_sizes ppfbs
                           ON ppf.product_pet_foods_breed_size_id = ppfbs.product_pet_food_breed_sizes_id
                 LEFT JOIN product_grooming_and_hygiene pgah ON p.product_id = pgah.product_id
                 LEFT JOIN product_feeders pf ON p.product_id = pf.product_id
                 LEFT JOIN product_feeder_types pft
                           ON pf.product_feeders_type_id = pft.product_feeder_types_id
                 LEFT JOIN product_feeder_materials pfm
                           ON pf.product_feeders_material_id = pfm.product_feeder_materials_id;
    END;
$$
