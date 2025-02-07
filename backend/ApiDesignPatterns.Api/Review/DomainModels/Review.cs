// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels;

public record Review
{
    public required long Id { get; init; }
    public required long ProductId { get; init; }
    public required decimal Rating { get; init; }
    public required string Text { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}
