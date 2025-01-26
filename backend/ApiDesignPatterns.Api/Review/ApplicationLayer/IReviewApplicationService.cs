// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.ReviewControllers;

namespace backend.Review.ApplicationLayer;

public interface IReviewApplicationService
{
    Task DeleteReviewAsync(long id);
    Task ReplaceReviewAsync(DomainModels.Review review);
    Task UpdateReviewAsync(UpdateReviewRequest request, DomainModels.Review review);
}
