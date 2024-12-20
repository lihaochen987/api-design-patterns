// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ReviewControllers;

public record ReplaceReviewRequest
{
    public string? ProductId { get; init; }
    public string? Rating { get; init; }
    public string? Text { get; init; }
    public string UpdatedAt { get; init; } = DateTimeOffset.UtcNow.ToString("o");
}
