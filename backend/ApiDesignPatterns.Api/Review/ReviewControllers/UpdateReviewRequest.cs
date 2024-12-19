// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ReviewControllers;

public class UpdateReviewRequest
{
    public string ProductId { get; init; } = "";
    public string Rating { get; init; } = "";
    public string Text { get; init; } = "";
    public string CreatedAt { get; init; } = "";
    public string UpdatedAt { get; init; } = "";
    public List<string> FieldMask { get; init; } = ["*"];
}
