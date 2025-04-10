// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Review.Controllers;

public record ReplaceReviewResponse
{
    [Required] public required string Id { get; init; }
    [Required] public required string ProductId { get; init; }
    [Required] public required string Rating { get; init; }
    [Required] public required string Text { get; init; }
    [Required] public required string CreatedAt { get; init; }
    [Required] public required string UpdatedAt { get; init; }
}
