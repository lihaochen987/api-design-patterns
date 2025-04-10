// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.Controllers;

public record ListReviewsRequest
{
    public string? Filter { get; init; }
    public string? PageToken { get; init; } = "";
    public int MaxPageSize { get; init; } = 10;
}
