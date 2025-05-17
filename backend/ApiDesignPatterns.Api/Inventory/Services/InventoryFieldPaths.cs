// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Inventory.Services;

public class InventoryFieldPaths
{
    public readonly HashSet<string> ValidPaths =
    [
        "*",
        "id",
        "userid",
        "productid",
        "quantity",
        "restockdate"
    ];
}
