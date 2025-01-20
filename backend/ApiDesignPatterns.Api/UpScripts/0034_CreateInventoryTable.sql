DO
$$
    BEGIN
        CREATE TABLE Inventory
        (
            inventory_id           SERIAL PRIMARY KEY,
            supplier_id            INT NOT NULL,
            product_id             INT NOT NULL,
            inventory_quantity     INT DEFAULT 0,
            inventory_restock_date DATE,

            CONSTRAINT fk_supplier
                FOREIGN KEY (supplier_id) REFERENCES suppliers (supplier_id)
                    ON DELETE CASCADE,

            CONSTRAINT fk_product
                FOREIGN KEY (product_id) REFERENCES products (product_id)
                    ON DELETE CASCADE,

            CONSTRAINT unique_supplier_product UNIQUE (supplier_id, product_id)
        );
    END;
$$
