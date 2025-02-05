// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels;
using backend.Review.ReviewControllers;
using backend.Shared.QueryHandler;

namespace backend.Review.ApplicationLayer.Queries.ListReviews;

public record ListReviewsQuery : IQuery<(List<ReviewView>, string?)>
{
    public required string ParentId { get; init; }
    public required ListReviewsRequest Request { get; init; }
}
