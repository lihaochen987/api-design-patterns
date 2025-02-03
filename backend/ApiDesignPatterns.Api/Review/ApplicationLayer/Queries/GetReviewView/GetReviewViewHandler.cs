// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels;
using backend.Review.InfrastructureLayer.Database.ReviewView;
using backend.Shared.QueryHandler;

namespace backend.Review.ApplicationLayer.Queries.GetReviewView;

public class GetReviewViewHandler(IReviewViewRepository repository) : IQueryHandler<GetReviewViewQuery, ReviewView>
{
    public async Task<ReviewView?> Handle(GetReviewViewQuery query)
    {
        ReviewView? review = await repository.GetReviewView(query.Id);
        return review;
    }
}
