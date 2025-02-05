// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Services;

public class ProductPricingFieldPaths
{
    public readonly HashSet<string> ValidPaths =
    [
        "*",
        "id",
        "pricing.*",
        "pricing.baseprice",
        "pricing.discountpercentage",
        "pricing.taxrate"
    ];
}
