-- Seed initial products data into 'products' table
DO
$$
    BEGIN
        INSERT INTO products (product_name, product_price, product_category,
                              product_dimensions_length_cm, product_dimensions_width_cm, product_dimensions_height_cm)
        VALUES ('Dry Dog Food', 50.00, 'PetFood', 10.0, 5.0, 3.0),
               ('Wet Dog Food', 35.00, 'PetFood', 8.0, 4.0, 3.0),
               ('Dog Treats', 10.00, 'PetFood', 5.0, 3.0, 1.0),
               ('Chew Toy', 15.00, 'Toys', 6.0, 6.0, 4.0),
               ('Fetch Ball', 8.00, 'Toys', 4.0, 4.0, 4.0),
               ('Dog Collar', 12.00, 'CollarsAndLeashes', 5.0, 1.0, 0.5),
               ('Dog Leash', 20.00, 'CollarsAndLeashes', 100.0, 2.0, 0.5),
               ('Dog Shampoo', 10.00, 'GroomingAndHygiene', 8.0, 4.0, 2.0),
               ('Dog Brush', 7.00, 'GroomingAndHygiene', 7.0, 3.0, 2.0),
               ('Comfort Dog Bed', 80.00, 'Beds', 60.0, 40.0, 10.0),
               ('Rope Tug Toy', 9.00, 'Toys', 8.0, 3.0, 3.0),
               ('Dog Bowl', 6.00, 'Feeders', 15.0, 15.0, 5.0),
               ('Automatic Feeder', 45.00, 'Feeders', 25.0, 25.0, 30.0),
               ('Dog Carrier', 65.00, 'TravelAccessories', 40.0, 30.0, 30.0),
               ('Dog Raincoat', 18.00, 'Clothing', 20.0, 10.0, 2.0),
               ('Dog Bandana', 5.00, 'Clothing', 20.0, 20.0, 0.2),
               ('Training Pads', 25.00, 'GroomingAndHygiene', 30.0, 30.0, 2.0),
               ('Dental Chews', 15.00, 'PetFood', 10.0, 5.0, 1.0),
               ('Cooling Mat', 35.00, 'Beds', 90.0, 50.0, 1.0),
               ('Reflective Harness', 25.00, 'CollarsAndLeashes', 20.0, 5.0, 1.0),
               ('Dry Dog Food', 50.00, 'PetFood', 10.0, 5.0, 3.0),
               ('Wet Dog Food', 35.00, 'PetFood', 8.0, 4.0, 3.0),
               ('Dog Treats', 10.00, 'PetFood', 5.0, 3.0, 1.0)
        ON CONFLICT (product_id) DO NOTHING;
    END
$$