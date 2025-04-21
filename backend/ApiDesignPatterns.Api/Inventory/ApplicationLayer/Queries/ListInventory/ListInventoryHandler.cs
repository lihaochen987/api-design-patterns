// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.DomainModels;
using backend.Inventory.InfrastructureLayer.Database.InventoryView;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.ListInventory;

public class ListInventoryHandler(IInventoryViewRepository repository)
    : IQueryHandler<ListInventoryQuery, PagedInventory>
{
    public async Task<PagedInventory> Handle(ListInventoryQuery query)
    {
        (List<InventoryView> inventory, string? nextPageToken) = await repository.ListInventoryAsync(
            query.PageToken,
            query.Filter,
            query.MaxPageSize);
        return new PagedInventory(inventory, nextPageToken);
    }
}
