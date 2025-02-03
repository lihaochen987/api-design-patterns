// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels;

public class Review
{
    public required long Id { get; set; }
    public required long ProductId { get; set; }
    public required decimal Rating { get; set; }
    public required string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
