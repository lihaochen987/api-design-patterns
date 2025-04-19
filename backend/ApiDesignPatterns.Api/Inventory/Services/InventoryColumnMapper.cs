// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Inventory.Services;

public class InventoryColumnMapper : IColumnMapper
{
    public string MapToColumnName(string propertyName)
    {
        return propertyName switch
        {
            "Id" => "inventory_id",
            "SupplierId" => "supplier_id",
            "ProductId" => "product_id",
            "Quantity" => "inventory_quantity",
            "RestockDate" => "inventory_restock_date",
            _ => throw new ArgumentException($"Invalid property name: {propertyName}")
        };
    }
}
