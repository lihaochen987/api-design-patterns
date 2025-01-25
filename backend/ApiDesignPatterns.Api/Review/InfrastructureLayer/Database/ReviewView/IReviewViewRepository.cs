// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Review.InfrastructureLayer.Database.ReviewView;

public interface IReviewViewRepository
{
    Task<DomainModels.ReviewView?> GetReviewView(long id);

    Task<(List<DomainModels.ReviewView>, string?)> ListReviewsAsync(
        string? pageToken,
        string? filter,
        int maxPageSize,
        string? parent);
}
