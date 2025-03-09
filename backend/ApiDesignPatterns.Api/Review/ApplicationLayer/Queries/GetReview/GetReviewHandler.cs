// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer.Database.Review;
using backend.Shared.QueryHandler;

namespace backend.Review.ApplicationLayer.Queries.GetReview;

public class GetReviewHandler(IReviewRepository repository) : IQueryHandler<GetReviewQuery, DomainModels.Review?>
{
    public async Task<DomainModels.Review?> Handle(GetReviewQuery query)
    {
        DomainModels.Review? review = await repository.GetReviewAsync(query.Id);

        return review ?? null;
    }
}
