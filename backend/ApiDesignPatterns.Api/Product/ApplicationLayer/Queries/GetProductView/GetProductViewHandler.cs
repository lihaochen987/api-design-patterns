// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetProductView;

public class GetProductViewHandler(IProductViewRepository repository) : IQueryHandler<GetProductViewQuery, ProductView>
{
    public async Task<ProductView?> Handle(GetProductViewQuery query)
    {
        ProductView? product = await repository.GetProductView(query.Id);
        return product;
    }
}
