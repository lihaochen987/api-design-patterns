// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.Controllers;

public class UpdateReviewRequest
{
    public string? ProductId { get; init; }
    public decimal? Rating { get; init; }
    public string? Text { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
