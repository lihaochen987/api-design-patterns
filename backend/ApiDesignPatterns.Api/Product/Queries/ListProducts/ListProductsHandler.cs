// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Shared.QueryHandler;

namespace backend.Product.Queries.ListProducts;

public class ListProductsHandler(IProductViewRepository repository)
    : IQueryHandler<ListProductsQuery, (List<ProductView>, string?)>
{
    public async Task<(List<ProductView>, string?)> Handle(ListProductsQuery query)
    {
        (List<ProductView> products, string? nextPageToken) = await repository.ListProductsAsync(
            query.PageToken,
            query.Filter,
            query.MaxPageSize);
        return (products, nextPageToken);
    }
}
