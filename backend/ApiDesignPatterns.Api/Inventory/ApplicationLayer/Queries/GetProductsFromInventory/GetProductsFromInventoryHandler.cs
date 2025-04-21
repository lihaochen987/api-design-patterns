// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;

namespace backend.Inventory.ApplicationLayer.Queries.GetProductsFromInventory;

public class GetProductsFromInventoryHandler : ISyncQueryHandler<GetProductsFromInventoryQuery, List<GetProductResponse?>>
{
    public List<GetProductResponse?> Handle(GetProductsFromInventoryQuery query)
    {
        var result = query.Products
            .Where(supplier => supplier != null)
            .ToList();
        return result;
    }
}
