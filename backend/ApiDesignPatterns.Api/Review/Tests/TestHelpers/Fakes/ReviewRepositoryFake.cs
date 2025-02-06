// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Review.InfrastructureLayer.Database.Review;

namespace backend.Review.Tests.TestHelpers.Fakes;

public class ReviewRepositoryFake : Collection<DomainModels.Review>, IReviewRepository
{
    public bool IsDirty { get; set; }
    public Dictionary<string, int> CallCount { get; } = new();

    private void IncrementCallCount(string methodName)
    {
        if (!CallCount.TryAdd(methodName, 1))
        {
            CallCount[methodName]++;
        }
    }

    public Task<DomainModels.Review?> GetReviewAsync(long id)
    {
        IncrementCallCount(nameof(GetReviewAsync));
        DomainModels.Review? review = this.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(review);
    }

    public Task CreateReviewAsync(DomainModels.Review review)
    {
        IncrementCallCount(nameof(CreateReviewAsync));
        IsDirty = true;
        Add(review);
        return Task.CompletedTask;
    }

    public Task DeleteReviewAsync(long id)
    {
        IncrementCallCount(nameof(DeleteReviewAsync));
        var review = this.FirstOrDefault(r => r.Id == id);
        if (review == null)
        {
            return Task.CompletedTask;
        }

        Remove(review);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task UpdateReviewAsync(DomainModels.Review review)
    {
        IncrementCallCount(nameof(UpdateReviewAsync));
        int index = IndexOf(this.FirstOrDefault(r => r.Id == review.Id) ??
                            throw new InvalidOperationException());
        this[index] = review;
        IsDirty = true;

        return Task.CompletedTask;
    }
}
