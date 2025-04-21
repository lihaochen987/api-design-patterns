// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetProductsFromInventory;

public class GetProductsFromInventoryQuery : IQuery<List<GetProductResponse?>>
{
    public required List<GetProductResponse?> Products { get; set; }
}
