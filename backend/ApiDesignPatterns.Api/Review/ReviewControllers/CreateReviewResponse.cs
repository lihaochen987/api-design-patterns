// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ReviewControllers;

public record CreateReviewResponse
{
    public required string ProductId { get; init; }
    public required string Rating { get; init; }
    public required string Text { get; init; }
    public required string CreatedAt { get; init; }
    public required string UpdatedAt { get; init; }
}
