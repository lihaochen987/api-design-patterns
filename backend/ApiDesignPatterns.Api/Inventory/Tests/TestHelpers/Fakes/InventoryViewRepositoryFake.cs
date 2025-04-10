// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.InventoryView;

namespace backend.Inventory.Tests.TestHelpers.Fakes;

public class InventoryViewRepositoryFake : Collection<InventoryView>, IInventoryViewRepository
{
    public Task<InventoryView?> GetInventoryView(long id)
    {
        InventoryView? inventoryView = this.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(inventoryView);
    }

    // public Task CreateInventoryAsync(DomainModels.Inventory inventory)
    // {
    //     IncrementCallCount(nameof(CreateInventoryAsync));
    //     IsDirty = true;
    //     Add(inventory);
    //     return Task.CompletedTask;
    // }

    // public Task<DomainModels.Review?> GetReviewAsync(long id)
    // {
    //     IncrementCallCount(nameof(GetReviewAsync));
    //     DomainModels.Review? review = this.FirstOrDefault(r => r.Id == id);
    //     return Task.FromResult(review);
    // }
    //
    // public Task DeleteReviewAsync(long id)
    // {
    //     IncrementCallCount(nameof(DeleteReviewAsync));
    //     var review = this.FirstOrDefault(r => r.Id == id);
    //     if (review == null)
    //     {
    //         return Task.CompletedTask;
    //     }
    //
    //     Remove(review);
    //     IsDirty = true;
    //     return Task.CompletedTask;
    // }
    //
    // public Task UpdateReviewAsync(DomainModels.Review review)
    // {
    //     IncrementCallCount(nameof(UpdateReviewAsync));
    //     int index = IndexOf(this.FirstOrDefault(r => r.Id == review.Id) ??
    //                         throw new InvalidOperationException());
    //     this[index] = review;
    //     IsDirty = true;
    //
    //     return Task.CompletedTask;
    // }
}
