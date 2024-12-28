// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels;

using System.ComponentModel.DataAnnotations;

public class Review
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    [Range(0, 5)] public decimal Rating { get; set; }
    [MaxLength(5000)] public required string Text { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}
