// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ApplicationLayer.Commands.CreateReview;

public class CreateReviewQuery
{
    public required DomainModels.Review Review { get; init; }
    public required long ProductId { get; init; }
}
