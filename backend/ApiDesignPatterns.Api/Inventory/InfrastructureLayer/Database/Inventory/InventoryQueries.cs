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
}
