DO
$$
    BEGIN
        ALTER TABLE inventory
            ALTER COLUMN user_id SET NOT NULL,
            ADD CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users ON DELETE CASCADE;

        ALTER TABLE inventory
            DROP CONSTRAINT unique_supplier_product;

        ALTER TABLE inventory
            ADD CONSTRAINT unique_user_product UNIQUE (user_id, product_id);

        ALTER TABLE inventory
            DROP COLUMN supplier_id;

        ALTER TABLE phone_numbers
            ALTER COLUMN user_id SET NOT NULL,
            ADD CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users ON DELETE CASCADE;

        ALTER TABLE phone_numbers
            DROP COLUMN supplier_id;

        ALTER TABLE addresses
            ALTER COLUMN user_id SET NOT NULL,
            ADD CONSTRAINT fk_user FOREIGN KEY (user_id) REFERENCES users ON DELETE CASCADE;

        ALTER TABLE addresses
            DROP COLUMN supplier_id;

        DROP TABLE IF EXISTS suppliers CASCADE ;
    END
$$;
