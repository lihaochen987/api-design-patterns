CREATE OR REPLACE FUNCTION update_products_view()
    RETURNS TRIGGER AS
$$
BEGIN
    UPDATE products
    SET product_name                = NEW.product_name,
        product_dimensions_length   = NEW.product_dimensions_length,
        product_dimensions_width    = NEW.product_dimensions_width,
        product_dimensions_height   = NEW.product_dimensions_height,
        product_base_price          = NEW.product_base_price,
        product_discount_percentage = NEW.product_discount_percentage,
        product_tax_rate            = NEW.product_tax_rate
    WHERE product_id = OLD.product_id;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_update_products_view
    INSTEAD OF UPDATE
    ON products_view
    FOR EACH ROW
EXECUTE FUNCTION update_products_view();

CREATE OR REPLACE FUNCTION delete_from_products_view()
    RETURNS TRIGGER AS $$
BEGIN
    DELETE FROM products
    WHERE product_id = OLD.product_id;

    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_delete_products_view
    INSTEAD OF DELETE ON products_view
    FOR EACH ROW
EXECUTE FUNCTION delete_from_products_view();