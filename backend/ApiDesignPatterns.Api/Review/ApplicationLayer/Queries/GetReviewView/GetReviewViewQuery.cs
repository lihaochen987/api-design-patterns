// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels;
using backend.Shared.QueryHandler;

namespace backend.Review.ApplicationLayer.Queries.GetReviewView;

public record GetReviewViewQuery: IQuery<ReviewView>
{
    public long Id { get; init; }
}
