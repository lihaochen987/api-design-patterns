// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Review.ReviewControllers;

public class ListReviewsResponse
{
    [Required] public IEnumerable<GetReviewResponse?> Results { get; init; } = [];
    [Required] public string? NextPageToken { get; init; }
}
