// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.Services;

public class ReviewFieldMaskConfiguration
{
    public readonly HashSet<string> ReviewFieldPaths =
    [
        "*",
        "id",
        "productid",
        "rating",
        "text",
        "createdat",
        "updatedat"
    ];
}
