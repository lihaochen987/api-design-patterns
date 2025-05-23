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
    public void AddInventoryViews(List<InventoryView> inventoryViews)
    {
        foreach (var inventoryView in inventoryViews)
        {
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

        // Pagination filter - fix the variable name to match real logic
        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenInventory))
        {
            query = query.Where(r => r.Id > lastSeenInventory);
        }

        // Custom filter
        if (!string.IsNullOrEmpty(filter))
        {
            if (filter.Contains("ProductId =="))
            {
                string value = filter.Split("==")[1].Trim();
                query = query.Where(s => s.ProductId == long.Parse(value));
            }
            if (filter.Contains("Quantity =="))
            {
                string value = filter.Split("==")[1].Trim();
                query = query.Where(s => s.Quantity == decimal.Parse(value));
            }
        }

        var inventory = query
            .OrderBy(r => r.Id)
            .Take(maxPageSize + 1)
            .ToList();

        List<InventoryView> paginatedInventory =
            paginateService.Paginate(inventory, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedInventory, nextPageToken));
    }
}
