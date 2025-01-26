// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ApplicationLayer.Commands.DeleteReview;

public record DeleteReviewQuery
{
    public long Id { get; init; }
}
