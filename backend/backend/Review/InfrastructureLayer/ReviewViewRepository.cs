// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Review.DomainModels.Views;
using backend.Review.InfrastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace backend.Review.InfrastructureLayer;

public class ReviewViewRepository(ReviewDbContext context) : IReviewViewRepository
{
    public async Task<ReviewView?> GetReviewView(long id) =>
        await context.Set<ReviewView>()
            .FirstOrDefaultAsync(p => p.Id == id);
}
