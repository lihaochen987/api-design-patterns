DO
$$
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'product_grooming_and_hygiene') THEN
            CREATE TABLE product_grooming_and_hygiene
            (
                product_id                                      bigint NOT NULL
                    CONSTRAINT fk_product_id
                        REFERENCES products (product_id)
                        ON DELETE CASCADE,
                product_grooming_and_hygiene_is_natural         bool   NOT NULL,
                product_grooming_and_hygiene_is_hypoallergenic  bool   NOT NULL,
                product_grooming_and_hygiene_usage_instructions text   NOT NULL,
                product_grooming_and_hygiene_is_cruelty_free    bool   NOT NULL,
                product_grooming_and_hygiene_safety_warnings    text   NOT NULL,
                UNIQUE (product_id)
            );
        END IF;
    END
$$;

INSERT INTO product_grooming_and_hygiene (product_id,
                                          product_grooming_and_hygiene_is_natural,
                                          product_grooming_and_hygiene_is_hypoallergenic,
                                          product_grooming_and_hygiene_usage_instructions,
                                          product_grooming_and_hygiene_is_cruelty_free,
                                          product_grooming_and_hygiene_safety_warnings)
VALUES (8, true, true, 'Apply a small amount to wet coat, lather, and rinse thoroughly.', true,
        'Avoid contact with eyes.'),
       (9, false, true, 'Use daily for brushing teeth. Gently apply to teeth and gums.', true,
        'Do not allow pet to swallow large amounts.')
    ,
       (17, true, false, 'Brush gently from roots to tips to detangle and remove loose fur.', false,
        'Keep away from heat sources.');
