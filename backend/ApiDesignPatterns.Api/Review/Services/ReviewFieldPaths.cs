// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.Services;

public class ReviewFieldPaths
{
    public readonly HashSet<string> ValidPaths =
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
