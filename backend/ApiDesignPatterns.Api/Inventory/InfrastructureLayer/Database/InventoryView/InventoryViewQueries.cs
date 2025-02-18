// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.InfrastructureLayer.Database.InventoryView;

public static class InventoryViewQueries
{
    public const string GetInventoryView = """
                                        SELECT
                                            inventory_id AS Id,
                                            supplier_id AS SupplierId,
                                            product_id AS ProductId,
                                            inventory_quantity AS Quantity,
                                            inventory_restock_date AS RestockDate
                                        FROM inventory_view
                                        WHERE inventory_id = @Id;
                                        """;
}
