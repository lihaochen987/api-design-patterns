// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.InventoryView;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Shared;

namespace backend.Inventory.Tests.TestHelpers.Fakes;

public class InventoryViewRepositoryFake(PaginateService<InventoryView> paginateService)
    : SortedSet<InventoryView>(
            Comparer<InventoryView>.Create((x, y) => ReferenceEquals(x, y) ? 0 : x.Id.CompareTo(y.Id))),
        IInventoryViewRepository
{
    public void AddInventoryView(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var inventoryView = new InventoryViewTestDataBuilder().Build();
            Add(inventoryView);
        }
    }

    public Task<InventoryView?> GetInventoryView(long id)
    {
        InventoryView? inventoryView = this.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(inventoryView);
    }

    public Task<(List<InventoryView>, string?)>
        ListInventoryAsync(string? pageToken, string? filter, int maxPageSize)
    {
        var query = this.AsEnumerable();

        // Pagination filter
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenReview))
        {
            query = query.Where(r => r.Id > lastSeenReview);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            if (filter.Contains("ProductId =="))
            {
                string value = filter.Split("==")[1].Trim();
                query = query.Where(s => s.ProductId == decimal.Parse(value));
            }
            else
            {
                throw new ArgumentException();
            }
        }

        var reviews = query
            .OrderBy(r => r.Id)
            .Take(maxPageSize + 1)
            .ToList();

        List<InventoryView> paginatedReviews =
            paginateService.Paginate(reviews, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedReviews, nextPageToken));
    }
}
