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

    public Task<(List<InventoryView>, string?)>
        ListInventoryAsync(string? pageToken, string? filter, int maxPageSize) => throw new NotImplementedException();
}
