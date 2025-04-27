-- Execute the migration in a transaction
DO $$
    DECLARE
        saved_view_definition TEXT;
    BEGIN
        -- Add extension if not already present
        CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

        -- Create a temporary UUID column
        ALTER TABLE inventory ADD COLUMN temp_id UUID DEFAULT uuid_generate_v4();

        -- Update the temp_id for existing rows
        UPDATE inventory SET temp_id = uuid_generate_v4();

        -- Drop the primary key constraint
        ALTER TABLE inventory DROP CONSTRAINT inventory_pkey;

        -- Drop the dependent view
        DROP VIEW inventory_view;

        -- Drop the original inventory_id column
        ALTER TABLE inventory DROP COLUMN inventory_id;

        -- Rename the UUID column to inventory_id
        ALTER TABLE inventory RENAME COLUMN temp_id TO inventory_id;

        -- Set the renamed column as primary key
        ALTER TABLE inventory ADD CONSTRAINT inventory_pkey PRIMARY KEY (inventory_id);
    END;
$$;
