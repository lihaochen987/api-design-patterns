// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ReviewControllers;

namespace backend.Review.ApplicationLayer.Commands.UpdateReview;

public class UpdateReviewCommand
{
    public required UpdateReviewRequest Request { get; set; }
    public required DomainModels.Review Review { get; set; }
}
