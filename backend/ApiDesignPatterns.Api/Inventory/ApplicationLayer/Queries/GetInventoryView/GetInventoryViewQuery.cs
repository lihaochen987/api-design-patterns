// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.DomainModels;
using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;

namespace backend.Inventory.ApplicationLayer.Queries.GetInventoryView;

public record GetInventoryViewQuery : IQuery<SupplierView>, IQuery<InventoryView?>
{
    public long Id { get; init; }
}
