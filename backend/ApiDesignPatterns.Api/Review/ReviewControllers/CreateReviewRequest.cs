// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace backend.Review.ReviewControllers;

public record CreateReviewRequest
{
    [Required] public required decimal Rating { get; init; }
    [Required] public required string Text { get; init; }
}
