namespace backend.Product;

using System.Collections.Generic;

public static class ProductMaskFieldPaths
{
    public static readonly HashSet<string> AllFieldPaths =
    [
        "*",
        "id",
        "name",
        "price",
        "category",
        "dimensions.*",
        "dimensions.width",
        "dimensions.height",
        "dimensions.length"
    ];
}