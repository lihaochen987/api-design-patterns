// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels;

internal record ReviewRow
{
    public long Id { get; init; }
    public long ProductId { get; init; }
    public decimal Rating { get; init; }
    public required string Text { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}
