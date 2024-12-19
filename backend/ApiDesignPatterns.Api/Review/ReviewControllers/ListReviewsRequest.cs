// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ReviewControllers;

public class ListReviewsRequest
{
    public string? Parent { get; set; }
    public string? Filter { get; set; }
    public string? PageToken { get; set; } = "";
    public int MaxPageSize { get; set; } = 10;
}
