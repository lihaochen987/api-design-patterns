// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.Controllers;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.ListInventory;

public class ListInventoryQuery : IQuery<PagedInventory>
{
    public required ListInventoryRequest Request { get; init; }
}
