DROP FUNCTION IF EXISTS alter_column_type(TEXT, TEXT, TEXT);

CREATE OR REPLACE FUNCTION alter_column_type(
    p_table_name TEXT,
    p_column_name TEXT,
    p_column_type TEXT
)
    RETURNS VOID AS $$
DECLARE
    max_length INTEGER;
BEGIN
    -- Extract the numeric part from the column type (e.g., 'VARCHAR(100)')
    max_length := NULLIF(regexp_replace(p_column_type, '[^0-9]', '', 'g'), '')::INTEGER;

    -- Check if the column already has the desired type
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_name = p_table_name
          AND column_name = p_column_name
          AND data_type = 'character varying'
          AND character_maximum_length = max_length
    ) THEN
        RAISE NOTICE 'Column "%s.%s" is already %s.', p_table_name, p_column_name, p_column_type;
    ELSE
        EXECUTE format(
                'ALTER TABLE %I ALTER COLUMN %I TYPE %s;',
                p_table_name,
                p_column_name,
                p_column_type
                );
        RAISE NOTICE 'Column "%s.%s" has been altered to %s.', p_table_name, p_column_name, p_column_type;
    END IF;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION create_or_replace_products_view()
    RETURNS VOID AS
$$
BEGIN
    EXECUTE $sql$
    CREATE OR REPLACE VIEW products_view AS
    SELECT p.product_id,
           p.product_name,
           p.product_dimensions_length_cm,
           p.product_dimensions_width_cm,
           p.product_dimensions_height_cm,
           c.product_category_name,
           ROUND(
               (p.product_base_price * (1 - p.product_discount_percentage / 100)) *
               (1 + p.product_tax_rate / 100),
               2
           ) AS product_price,
           ppfag.product_pet_food_age_group,
           ppfbs.product_pet_food_breed_size,
           ppf.product_pet_foods_ingredients,
           ppf.product_pet_foods_nutritional_info,
           ppf.product_pet_foods_storage_instructions,
           ppf.product_pet_foods_weight_kg,
           pgah.product_grooming_and_hygiene_is_natural,
           pgah.product_grooming_and_hygiene_is_hypoallergenic,
           pgah.product_grooming_and_hygiene_usage_instructions,
           pgah.product_grooming_and_hygiene_is_cruelty_free,
           pgah.product_grooming_and_hygiene_safety_warnings
    FROM products p
             LEFT JOIN product_categories c
             ON p.product_category = c.product_category_id
             LEFT JOIN product_pet_foods ppf
             ON p.product_id = ppf.product_id
             LEFT JOIN product_pet_food_age_groups ppfag
             ON ppf.product_pet_foods_age_group_id = ppfag.product_pet_food_age_groups_id
             LEFT JOIN product_pet_food_breed_sizes ppfbs
             ON ppf.product_pet_foods_breed_size_id = ppfbs.product_pet_food_breed_sizes_id
             LEFT JOIN product_grooming_and_hygiene pgah
             ON p.product_id = pgah.product_id;
    $sql$;
    RAISE NOTICE 'View "products_view" has been created or replaced.';
END;
$$ LANGUAGE plpgsql;

SELECT alter_column_type('products', 'product_name', 'VARCHAR(100)');
SELECT alter_column_type('product_pet_foods', 'product_pet_foods_ingredients', 'VARCHAR(300)');
SELECT alter_column_type('product_pet_foods', 'product_pet_foods_storage_instructions', 'VARCHAR(300)');
SELECT alter_column_type('product_grooming_and_hygiene', 'product_grooming_and_hygiene_usage_instructions',
                         'VARCHAR(300)');
SELECT alter_column_type('product_grooming_and_hygiene', 'product_grooming_and_hygiene_safety_warnings',
                         'VARCHAR(300)');
SELECT create_or_replace_products_view();

DROP FUNCTION IF EXISTS alter_column_type(TEXT, TEXT, TEXT);
DROP FUNCTION IF EXISTS create_or_replace_products_view();