// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;

namespace backend.Review.DomainModels;

public class ReviewView : Identifier
{
    public required long ProductId { get; set; }
    public required decimal Rating { get; set; }
    public required string Text { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
