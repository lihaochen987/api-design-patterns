// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.DomainModels;

using System.ComponentModel.DataAnnotations;

public class Review
{
    [Required] public required long Id { get; set; }
    [Required] public required long ProductId { get; set; }
    [Required] [Range(0, 5)] public required decimal Rating { get; set; }
    [Required] [MaxLength(5000)] public required string Text { get; set; }
    [Required] public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
