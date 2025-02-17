DO
$$
    BEGIN
        CREATE OR REPLACE VIEW inventory_view AS
        SELECT i.inventory_id,
               i.supplier_id,
               i.product_id,
               i.inventory_quantity,
               i.inventory_restock_date
        FROM inventory i;
    END;
$$
