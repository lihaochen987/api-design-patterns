DO
$$
    BEGIN
        -- Populate product_review_summaries table with specific values for some products
        -- and default values (0) for others

        INSERT INTO product_review_summaries (product_id, product_one_star, product_two_stars, product_three_stars,
                                              product_four_stars, product_five_stars)
        SELECT product_id,
               product_one_star,
               product_two_stars,
               product_three_stars,
               product_four_stars,
               product_five_stars
        FROM (VALUES (1, 0, 1, 2, 3, 4),  -- Dry Dog Food: Example distribution
                     (2, 1, 0, 1, 3, 5),  -- Wet Dog Food
                     (3, 0, 0, 0, 2, 8),  -- Dog Treats: Mostly 5-star ratings
                     (4, 2, 2, 1, 1, 4),  -- Chew Toy
                     (5, 0, 1, 1, 2, 3),  -- Fetch Ball
                     (6, 1, 1, 2, 0, 4),  -- Dog Collar
                     (7, 0, 0, 3, 4, 5),  -- Dog Leash
                     (8, 0, 0, 0, 3, 7),  -- Dog Shampoo: Mostly 5-star ratings
                     (9, 0, 0, 2, 2, 4),  -- Dog Brush
                     (10, 1, 1, 1, 3, 4), -- Comfort Dog Bed
                     (11, 0, 1, 2, 1, 5), -- Rope Tug Toy
                     (12, 0, 0, 1, 4, 5), -- Dog Bowl: Mostly positive ratings
                     (13, 1, 0, 1, 2, 3), -- Automatic Feeder
                     (14, 0, 2, 3, 3, 2), -- Dog Carrier: Mixed ratings
                     (15, 1, 1, 2, 2, 4), -- Dog Raincoat
                     (16, 0, 0, 1, 1, 8), -- Dog Bandana: Mostly 5-star ratings
                     (17, 2, 1, 1, 2, 1), -- Training Pads: Mixed ratings
                     (18, 0, 0, 1, 4, 5), -- Dental Chews
                     (19, 0, 0, 0, 3, 7), -- Cooling Mat: Mostly 5-star ratings
                     (20, 0, 1, 1, 3, 5), -- Reflective Harness
                     (21, 0, 1, 2, 3, 4), -- Dry Dog Food (duplicate, example values)
                     (22, 1, 0, 1, 3, 5), -- Wet Dog Food (duplicate, example values)
                     (23, 0, 0, 0, 2, 8) -- Dog Treats (duplicate, mostly 5-star ratings)
             ) AS new_data(product_id, product_one_star, product_two_stars, product_three_stars, product_four_stars,
                           product_five_stars)
        WHERE NOT EXISTS (SELECT 1 FROM product_review_summaries prs WHERE prs.product_id = new_data.product_id);

    END
$$