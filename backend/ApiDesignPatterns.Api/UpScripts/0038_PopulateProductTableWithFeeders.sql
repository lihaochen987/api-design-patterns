-- Add more feeder products to the 'products' table
DO
$$
    BEGIN
        INSERT INTO products (product_name, product_base_price, product_discount_percentage, product_tax_rate, product_category,
                              product_dimensions_length_cm, product_dimensions_width_cm, product_dimensions_height_cm)
        VALUES
            ('Smart Pet Feeder Pro', 89.99, 5.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             35.0, 25.0, 40.0),

            ('Double Dog Bowl Set', 22.50, 0.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             30.0, 15.0, 5.0),

            ('Travel Portable Feeder', 18.75, 15.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             20.0, 15.0, 8.0),

            ('Slow Feeder Bowl', 14.99, 0.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             18.0, 18.0, 4.0),

            ('Gravity Pet Food Dispenser', 32.00, 10.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             25.0, 20.0, 45.0),

            ('Elevated Feeding Station', 45.99, 0.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             40.0, 20.0, 15.0),

            ('Automatic Water Dispenser', 29.50, 5.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             30.0, 25.0, 35.0),

            ('Ceramic Designer Bowl', 19.99, 0.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             15.0, 15.0, 6.0),

            ('Programmable 5-Meal Feeder', 65.00, 8.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             28.0, 28.0, 30.0),

            ('Stainless Steel Bowl Set', 24.99, 0.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             22.0, 12.0, 5.0),

            ('Heavy Duty Raised Feeder', 38.50, 0.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             35.0, 18.0, 12.0),

            ('Silicone Collapsible Bowl', 11.99, 10.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             15.0, 15.0, 2.0),

            ('Anti-Slip Dog Dish', 16.50, 0.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             18.0, 18.0, 4.0),

            ('Automatic Treat Dispenser', 49.99, 15.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             20.0, 15.0, 25.0),

            ('Pet Water Fountain', 34.99, 5.0, 10.0,
             (SELECT product_category_id FROM product_categories WHERE product_category_name = 'Feeders'),
             25.0, 20.0, 18.0)
        ON CONFLICT (product_id) DO NOTHING;
    END
$$;
