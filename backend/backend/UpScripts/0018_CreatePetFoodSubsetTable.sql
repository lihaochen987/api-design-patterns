DO
$$
    BEGIN
        -- product_pet_food_age_groups validation table
        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'product_pet_food_age_groups') THEN
            CREATE TABLE product_pet_food_age_groups
            (
                product_pet_food_age_groups_id serial PRIMARY KEY,
                product_pet_food_age_group     text UNIQUE NOT NULL
            );
        END IF;

        INSERT INTO product_pet_food_age_groups (product_pet_food_age_group)
        VALUES ('Puppy'),
               ('Adult'),
               ('Senior')
        ON CONFLICT (product_pet_food_age_group) DO NOTHING;

        -- product_pet_food_breed_size validation table
        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'product_pet_food_breed_sizes') THEN
            CREATE TABLE product_pet_food_breed_sizes
            (
                product_pet_food_breed_sizes_id serial PRIMARY KEY,
                product_pet_food_breed_size     text UNIQUE NOT NULL
            );
        END IF;

        INSERT INTO product_pet_food_breed_sizes (product_pet_food_breed_size)
        VALUES ('Small'),
               ('Medium'),
               ('Large')
        ON CONFLICT (product_pet_food_breed_size) DO NOTHING;

        -- product_pet_food data table
        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'product_pet_food') THEN
            CREATE TABLE product_pet_food
            (
                product_id                            bigint  NOT NULL
                    CONSTRAINT fk_product_id
                        REFERENCES products (product_id)
                        ON DELETE CASCADE,
                product_pet_food_age_group_id         int     NOT NULL
                    CONSTRAINT fk_product_pet_food_age_group
                        REFERENCES product_pet_food_age_groups (product_pet_food_age_groups_id),
                product_pet_food_breed_size_id        int     NOT NULL
                    CONSTRAINT fk_product_pet_food_breed_size
                        REFERENCES product_pet_food_breed_sizes (product_pet_food_breed_sizes_id),
                product_pet_food_nutritional_info     jsonb,
                product_pet_food_ingredients          text    NOT NULL,
                product_pet_food_weight               numeric NOT NULL,
                product_pet_food_storage_instructions text
            );
        END IF;
    END
$$;
