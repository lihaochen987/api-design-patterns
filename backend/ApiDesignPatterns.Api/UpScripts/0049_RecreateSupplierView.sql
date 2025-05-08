DO
$$
    BEGIN
        DROP VIEW IF EXISTS suppliers_view;

        CREATE OR REPLACE VIEW suppliers_view AS
        SELECT s.supplier_id,
               CONCAT(s.supplier_firstname, ' ', s.supplier_lastname) AS supplier_fullname,
               s.supplier_email,
               s.supplier_created_at
        FROM suppliers s;
    END
$$;
