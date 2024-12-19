// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Review.ReviewControllers;

public class CreateReviewRequest
{
    [Required] public required string ProductId { get; init; }
    [Required] public required string Rating { get; init; }
    [Required] public required string Text { get; init; }
    public string CreatedAt { get; init; } = DateTimeOffset.UtcNow.ToString("o");
}
