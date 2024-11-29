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
               ppf.product_pet_foods_weight_kg
        FROM products p
                 LEFT JOIN
             product_categories c
             ON
                 p.product_category = c.product_category_id
                 LEFT JOIN product_pet_foods ppf ON p.product_id = ppf.product_id
                 LEFT JOIN product_pet_food_age_groups ppfag
                           ON ppf.product_pet_foods_age_group_id = ppfag.product_pet_food_age_groups_id
                 LEFT JOIN product_pet_food_breed_sizes ppfbs
                           ON ppf.product_pet_foods_breed_size_id = ppfbs.product_pet_food_breed_sizes_id;

    END;
$$
