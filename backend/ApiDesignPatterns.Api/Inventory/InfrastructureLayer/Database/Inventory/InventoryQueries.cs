// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.InfrastructureLayer.Database.Inventory;

public class InventoryQueries
{
    public const string CreateInventory = """
                                                  INSERT INTO inventory (
                                                      supplier_id,
                                                      product_id,
                                                      inventory_quantity,
                                                      inventory_restock_date)
                                                  VALUES (
                                                      @SupplierId,
                                                      @ProductId,
                                                      @Quantity,
                                                      @RestockDate
                                                      )
                                                  RETURNING inventory_id;
                                          """;

    public const string DeleteInventory = """
                                          DELETE FROM inventory
                                          WHERE inventory_id = @Id;
                                          """;

    public const string GetInventoryById = """
                                           SELECT
                                               inventory_id AS Id,
                                               supplier_id AS SupplierId,
                                               product_id AS ProductId,
                                               inventory_quantity AS Quantity,
                                               inventory_restock_date AS RestockDate
                                           FROM inventory
                                           WHERE inventory_id = @Id;
                                           """;

    public const string GetInventoryByProductAndSupplier = """
                                                           SELECT
                                                               inventory_id AS Id,
                                                               supplier_id AS SupplierId,
                                                               product_id AS ProductId,
                                                               inventory_quantity AS Quantity,
                                                               inventory_restock_date AS RestockDate
                                                           FROM inventory
                                                           WHERE supplier_id = @SupplierId AND product_id = @ProductId;
                                                           """;

    public const string UpdateInventory = """

                                                  UPDATE inventory
                                                  SET
                                                      inventory_quantity = @Quantity,
                                                      inventory_restock_date = @RestockDate
                                                  WHERE inventory_id = @Id;
                                          """;
}
