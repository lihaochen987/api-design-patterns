// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.ApplicationLayer;

public interface IReviewApplicationService
{
    Task<DomainModels.Review?> GetReviewAsync(long id);
    Task CreateReviewAsync(DomainModels.Review review);
    Task DeleteReviewAsync(DomainModels.Review review);
    Task UpdateReviewAsync(DomainModels.Review review);
}
