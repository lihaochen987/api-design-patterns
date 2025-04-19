// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Inventory.InfrastructureLayer.Database.Inventory;

namespace backend.Inventory.Tests.TestHelpers.Fakes;

public class InventoryRepositoryFake : Collection<DomainModels.Inventory>, IInventoryRepository
{
    public bool IsDirty { get; set; }

    public Task CreateInventoryAsync(DomainModels.Inventory inventory)
    {
        IsDirty = true;
        Add(inventory);
        return Task.CompletedTask;
    }

    public Task<DomainModels.Inventory?> GetInventoryByIdAsync(long id)
    {
        DomainModels.Inventory? inventory = this.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(inventory);
    }

    public Task UpdateInventoryAsync(DomainModels.Inventory inventory)
    {
        int index = IndexOf(this.FirstOrDefault(r => r.Id == inventory.Id) ??
                            throw new InvalidOperationException());
        this[index] = inventory;
        IsDirty = true;

        return Task.CompletedTask;
    }

    public Task<DomainModels.Inventory?> GetInventoryByProductAndSupplierAsync(long productId, long supplierId)
    {
        DomainModels.Inventory? inventory =
            this.FirstOrDefault(r => r.ProductId == productId && r.SupplierId == supplierId);
        return Task.FromResult(inventory);
    }

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
