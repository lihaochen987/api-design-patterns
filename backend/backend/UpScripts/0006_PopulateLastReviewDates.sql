DO
$$
    BEGIN
        -- Populate product_last_reviewed_date and product_last_reviewed_time for all 23 products

        UPDATE products
        SET product_last_reviewed_date = '2024-01-10',
            product_last_reviewed_time = '10:30:00'
        WHERE product_id = 1
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-11',
            product_last_reviewed_time = '11:15:00'
        WHERE product_id = 2
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-12',
            product_last_reviewed_time = '09:45:00'
        WHERE product_id = 3
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-13',
            product_last_reviewed_time = '14:00:00'
        WHERE product_id = 4
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-14',
            product_last_reviewed_time = '16:30:00'
        WHERE product_id = 5
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-15',
            product_last_reviewed_time = '08:20:00'
        WHERE product_id = 6
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-16',
            product_last_reviewed_time = '12:00:00'
        WHERE product_id = 7
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-17',
            product_last_reviewed_time = '15:45:00'
        WHERE product_id = 8
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-18',
            product_last_reviewed_time = '10:10:00'
        WHERE product_id = 9
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-19',
            product_last_reviewed_time = '13:30:00'
        WHERE product_id = 10
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-20',
            product_last_reviewed_time = '14:20:00'
        WHERE product_id = 11
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-21',
            product_last_reviewed_time = '08:55:00'
        WHERE product_id = 12
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-22',
            product_last_reviewed_time = '09:15:00'
        WHERE product_id = 13
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-23',
            product_last_reviewed_time = '11:35:00'
        WHERE product_id = 14
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-24',
            product_last_reviewed_time = '10:05:00'
        WHERE product_id = 15
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-25',
            product_last_reviewed_time = '12:45:00'
        WHERE product_id = 16
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-26',
            product_last_reviewed_time = '13:55:00'
        WHERE product_id = 17
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-27',
            product_last_reviewed_time = '15:05:00'
        WHERE product_id = 18
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-28',
            product_last_reviewed_time = '16:15:00'
        WHERE product_id = 19
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-29',
            product_last_reviewed_time = '10:25:00'
        WHERE product_id = 20
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-30',
            product_last_reviewed_time = '11:35:00'
        WHERE product_id = 21
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-01-31',
            product_last_reviewed_time = '14:45:00'
        WHERE product_id = 22
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

        UPDATE products
        SET product_last_reviewed_date = '2024-02-01',
            product_last_reviewed_time = '09:55:00'
        WHERE product_id = 23
          AND product_last_reviewed_date IS NULL
          AND product_last_reviewed_time IS NULL;

    END;
$$;
