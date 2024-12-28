// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels;
using backend.Review.ReviewControllers;

namespace backend.Review.ApplicationLayer;

public interface IReviewViewApplicationService
{
    Task<ReviewView?> GetReviewView(long id);
    Task<(List<ReviewView>, string?)> ListProductsAsync(ListReviewsRequest request);
}
