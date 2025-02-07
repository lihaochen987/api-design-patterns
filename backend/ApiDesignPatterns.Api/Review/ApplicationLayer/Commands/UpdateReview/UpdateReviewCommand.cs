// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ReviewControllers;

namespace backend.Review.ApplicationLayer.Commands.UpdateReview;

public record UpdateReviewCommand
{
    public required UpdateReviewRequest Request { get; init; }
    public required DomainModels.Review Review { get; init; }
}
