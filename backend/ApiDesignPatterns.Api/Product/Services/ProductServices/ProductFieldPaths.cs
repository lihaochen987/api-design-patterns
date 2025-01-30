// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Services.ProductServices;

public class ProductFieldPaths
{
    public HashSet<string> ValidPaths { get; } =
    [
        "*",
        "id",
        "name",
        "category",
        "price",
        "dimensions.*",
        "dimensions.width",
        "dimensions.height",
        "dimensions.length",
        "agegroup",
        "breedsize",
        "nutritionalinfo",
        "ingredients",
        "weightkg",
        "storageinstructions",
        "isnatural",
        "ishypoallergenic",
        "usageinstructions",
        "iscrueltyfree",
        "safetywarnings"
    ];
}
