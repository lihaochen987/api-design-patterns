// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using backend.Shared;

namespace backend.Review.DomainModels;

public class ReviewView : Identifier
{
    [Required] public required long ProductId { get; set; }
    [Required] public required decimal Rating { get; set; }
    [Required] public required string Text { get; set; }
    [Required] public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
