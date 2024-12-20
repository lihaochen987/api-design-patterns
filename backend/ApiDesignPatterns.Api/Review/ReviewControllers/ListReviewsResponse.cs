// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ReviewControllers;

public record ListReviewsResponse
{
    public IEnumerable<GetReviewResponse?> Results { get; init; } = [];
    public string? NextPageToken { get; init; }
}
