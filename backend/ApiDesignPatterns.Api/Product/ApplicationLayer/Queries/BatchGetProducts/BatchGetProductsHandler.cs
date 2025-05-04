// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProducts;

public class BatchGetProductsHandler(IProductRepository repository)
    : IAsyncQueryHandler<BatchGetProductsQuery, Result<List<DomainModels.Product>>>
{
    public async Task<Result<List<DomainModels.Product>>> Handle(BatchGetProductsQuery query)
    {
        var products = await repository.GetProductsByIds(query.ProductIds);

        var missingProductIds = query.ProductIds
            .Where(id => products.All(p => p.Id != id))
            .ToList();

        if (missingProductIds.Count != 0)
        {
            return Result.Failure<List<DomainModels.Product>>(
                $"Products not found: {string.Join(", ", missingProductIds)}");
        }
        return Result.Success(products);
    }
}
