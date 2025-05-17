DO
$$
    BEGIN
        INSERT INTO users (user_id,
                           user_firstname,
                           user_lastname,
                           user_email,
                           user_username,
                           user_password_hash,
                           user_created_at)
        SELECT s.supplier_id,
               s.supplier_firstname,
               s.supplier_lastname,
               s.supplier_email,
               LOWER(CONCAT(
                   SUBSTRING(s.supplier_firstname, 1, 1),
                   s.supplier_lastname
                     )) AS username,
               'defaultHashPlaceholder',
               s.supplier_created_at
        FROM suppliers s
        WHERE NOT EXISTS (SELECT 1
                          FROM users u
                          WHERE u.user_id = s.supplier_id);

        UPDATE addresses
        SET user_id = supplier_id
        WHERE user_id IS NULL
          AND supplier_id IS NOT NULL;

        UPDATE phone_numbers
        SET user_id = supplier_id
        WHERE user_id IS NULL
          AND supplier_id IS NOT NULL;

        UPDATE inventory
        SET user_id = supplier_id
        WHERE user_id IS NULL
          AND supplier_id IS NOT NULL;

        PERFORM setval(
                   pg_get_serial_sequence('users', 'user_id'),
                   COALESCE((SELECT MAX(user_id) FROM users), 0) + 1,
                   false
               );
    END
$$;
