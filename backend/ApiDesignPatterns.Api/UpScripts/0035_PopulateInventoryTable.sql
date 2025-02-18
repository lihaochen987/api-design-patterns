DO
$$
    BEGIN
        INSERT INTO public.inventory (supplier_id, product_id, inventory_quantity, inventory_restock_date)
        VALUES (1, 23, 150, '2025-03-01'),
               (1, 22, 75, '2025-03-15'),
               (2, 21, 200, '2025-02-28'),
               (2, 20, 100, '2025-03-10'),
               (3, 19, 300, '2025-03-05'),
               (3, 18, 50, '2025-03-20'),
               (4, 17, 175, '2025-03-12'),
               (4, 16, 225, '2025-02-25'),
               (5, 15, 80, '2025-03-08'),
               (5, 14, 120, '2025-03-18')
        ON CONFLICT (supplier_id, product_id)
            DO UPDATE SET inventory_quantity     = EXCLUDED.inventory_quantity,
                          inventory_restock_date = EXCLUDED.inventory_restock_date;
    END;
$$
