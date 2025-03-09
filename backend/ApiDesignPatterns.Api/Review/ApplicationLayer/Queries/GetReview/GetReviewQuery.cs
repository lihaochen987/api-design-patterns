// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Review.ApplicationLayer.Queries.GetReview;

public class GetReviewQuery : IQuery<DomainModels.Review?>
{
    public required long Id { get; init; }
}
