DO
$$
    BEGIN
        -- product_feeder_types validation table
        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'product_feeder_types') THEN
            CREATE TABLE product_feeder_types
            (
                product_feeder_types_id serial PRIMARY KEY,
                product_feeder_type     text UNIQUE NOT NULL
            );
        END IF;

        INSERT INTO product_feeder_types (product_feeder_type)
        VALUES ('Automatic'),
               ('Gravity'),
               ('Smart'),
               ('Manual')
        ON CONFLICT (product_feeder_type) DO NOTHING;

        -- product_feeder_materials validation table
        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'product_feeder_materials') THEN
            CREATE TABLE product_feeder_materials
            (
                product_feeder_materials_id serial PRIMARY KEY,
                product_feeder_material     text UNIQUE NOT NULL
            );
        END IF;

        INSERT INTO product_feeder_materials (product_feeder_material)
        VALUES ('Plastic'),
               ('Metal'),
               ('Ceramic'),
               ('Silicone')
        ON CONFLICT (product_feeder_material) DO NOTHING;

        -- product_feeders data table
        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'product_feeders') THEN
            CREATE TABLE product_feeders
            (
                product_id                            bigint  NOT NULL
                    CONSTRAINT fk_product_id
                        REFERENCES products (product_id)
                        ON DELETE CASCADE,
                product_feeders_type_id               int     NOT NULL
                    CONSTRAINT fk_product_feeder_type
                        REFERENCES product_feeder_types (product_feeder_types_id),
                product_feeders_material_id           int     NOT NULL
                    CONSTRAINT fk_product_feeder_material
                        REFERENCES product_feeder_materials (product_feeder_materials_id),
                product_feeders_capacity_liters       numeric NOT NULL,
                product_feeders_dishwasher_safe       boolean NOT NULL DEFAULT false,
                product_feeders_battery_operated      boolean NOT NULL DEFAULT false,
                product_feeders_battery_type          text,
                product_feeders_cleaning_instructions text,
                UNIQUE (product_id)
            );
        END IF;

        ALTER TABLE product_feeders
            ADD CONSTRAINT chk_product_feeders_capacity
                CHECK (product_feeders_capacity_liters > 0 AND product_feeders_capacity_liters <= 50);

        ALTER TABLE product_feeders
            ADD CONSTRAINT chk_product_feeders_battery_type
                CHECK (
                    (product_feeders_battery_operated = false AND product_feeders_battery_type IS NULL) OR
                    (product_feeders_battery_operated = true AND product_feeders_battery_type IS NOT NULL)
                    );
    END
$$;
