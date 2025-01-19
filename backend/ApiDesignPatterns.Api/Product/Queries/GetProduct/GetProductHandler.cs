// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared.QueryHandler;

namespace backend.Product.Queries.GetProduct;

public class GetProductHandler(IProductRepository repository) : IQueryHandler<GetProductQuery, DomainModels.Product>
{
    public async Task<DomainModels.Product?> Handle(GetProductQuery query)
    {
        DomainModels.Product? product = await repository.GetProductAsync(query.Id);
        return product ?? null;
    }
}
