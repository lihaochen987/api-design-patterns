DO
$$
    BEGIN
        CREATE TABLE IF NOT EXISTS supplier_phone_numbers
        (
            supplier_id                 BIGINT
                CONSTRAINT fk_supplier_id
                    REFERENCES suppliers (supplier_id)
                    ON DELETE CASCADE,
            supplier_phone_country_code VARCHAR(10),
            supplier_phone_area_code    VARCHAR(10),
            supplier_phone_number       VARCHAR(50)
        );

        IF NOT EXISTS (SELECT 1
                       FROM information_schema.table_constraints
                       WHERE table_name = 'supplier_phone_numbers'
                         AND constraint_name = 'chk_supplier_phone_country_code') THEN
            ALTER TABLE supplier_phone_numbers
                ADD CONSTRAINT chk_supplier_phone_country_code
                    CHECK (supplier_phone_country_code ~ '^\+\d+$');
        END IF;

        IF NOT EXISTS (SELECT 1
                       FROM information_schema.table_constraints
                       WHERE table_name = 'supplier_phone_numbers'
                         AND constraint_name = 'chk_supplier_phone_area_code') THEN
            ALTER TABLE supplier_phone_numbers
                ADD CONSTRAINT chk_phone_area_code CHECK (supplier_phone_area_code ~ '^\d{2,5}$');
        END IF;

        IF NOT EXISTS (SELECT 1
                       FROM information_schema.table_constraints
                       WHERE table_name = 'supplier_phone_numbers'
                         AND constraint_name = 'chk_supplier_phone_number') THEN
            ALTER TABLE supplier_phone_numbers
                ADD CONSTRAINT chk_supplier_phone_number CHECK (supplier_phone_number ~ '^\d{7,15}$');
        END IF;
    END
$$
