INSERT INTO product_pet_foods (product_id, product_pet_foods_age_group_id, product_pet_foods_breed_size_id,
                               product_pet_foods_nutritional_info, product_pet_foods_ingredients,
                               product_pet_foods_weight_kg,
                               product_pet_foods_storage_instructions)
VALUES ((SELECT product_id FROM products WHERE product_id = 1), 1, 1, '{
  "protein": 25,
  "fat": 10
}',
        'Chicken, Rice, Vitamins', 5.0, 'Keep in a cool, dry place'),

       ((SELECT product_id FROM products WHERE product_id = 2), 1, 2, '{
         "protein": 20,
         "fat": 8
       }',
        'Beef, Carrots, Peas', 1.2, 'Refrigerate after opening'),

       ((SELECT product_id FROM products WHERE product_id = 3), 1, 1, '{
         "protein": 15,
         "fat": 5
       }',
        'Peanut Butter, Oats', 0.5, 'Store in an airtight container'),

       ((SELECT product_id FROM products WHERE product_id = 18), 1, 3, '{
         "protein": 10,
         "fat": 4
       }',
        'Mint, Gelatin, Calcium', 0.2, 'Store in a dry place'),

       ((SELECT product_id FROM products WHERE product_id = 21), 1, 1, '{
         "protein": 28,
         "fat": 12
       }',
        'Lamb, Sweet Potato, Flaxseed', 5.0, 'Store in a cool, dry place'),

       ((SELECT product_id FROM products WHERE product_id = 22), 1, 2, '{
         "protein": 22,
         "fat": 9
       }',
        'Turkey, Brown Rice, Carrots', 4.5, 'Refrigerate after opening'),

       ((SELECT product_id FROM products WHERE product_id = 23), 1, 3, '{
         "protein": 18,
         "fat": 6
       }',
        'Salmon, Quinoa, Spinach', 6.0, 'Store in an airtight container');
