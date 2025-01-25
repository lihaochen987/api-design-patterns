// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.InfrastructureLayer.Database.Review;

public interface IReviewRepository
{
    Task<DomainModels.Review?> GetReviewAsync(long id);
    Task CreateReviewAsync(DomainModels.Review review);
    Task DeleteReviewAsync(long id);
    Task UpdateReviewAsync(DomainModels.Review review);
}
