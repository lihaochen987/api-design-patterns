// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.ListProducts;

public class ListProductsHandler(IProductViewRepository repository)
    : IAsyncQueryHandler<ListProductsQuery, PagedProducts>
{
    public async Task<PagedProducts> Handle(ListProductsQuery query)
    {
        var pagedProducts = await repository.ListProductsAsync(
            query.PageToken,
            query.Filter,
            query.MaxPageSize);
        return pagedProducts;
    }
}
