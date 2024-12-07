// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.InfrastructureLayer;

namespace backend.Review.ApplicationLayer;

public class ReviewApplicationService(IReviewRepository repository) : IReviewApplicationService
{
    public async Task<DomainModels.Review?> GetReviewAsync(long id)
    {
        DomainModels.Review? review = await repository.GetReviewAsync(id);

        return review ?? null;
    }

    public async Task DeleteReviewAsync(DomainModels.Review review) => await repository.DeleteReviewAsync(review);
}
