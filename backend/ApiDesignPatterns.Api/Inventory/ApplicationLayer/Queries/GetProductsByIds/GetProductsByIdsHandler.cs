// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetProductsByIds;

public class GetProductsByIdsHandler(IProductViewRepository repository)
    : IAsyncQueryHandler<GetProductsByIdsQuery, List<ProductView>>
{
    public async Task<List<ProductView>> Handle(GetProductsByIdsQuery query)
    {
        var products = await repository.GetProductsByIds(query.ProductIds);
        return products;
    }
}
