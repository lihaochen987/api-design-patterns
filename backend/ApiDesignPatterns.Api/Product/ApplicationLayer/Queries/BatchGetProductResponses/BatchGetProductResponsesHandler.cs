// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.Services.Mappers;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProductResponses;

public class BatchGetProductResponsesHandler(IProductViewRepository repository, IProductTypeMapper mapper)
    : IAsyncQueryHandler<BatchGetProductResponsesQuery, Result<List<Controllers.Product.GetProductResponse>>>
{
    public async Task<Result<List<Controllers.Product.GetProductResponse>>> Handle(BatchGetProductResponsesQuery query)
    {
        var products = await repository.GetProductsByIds(query.ProductIds);

        var missingProductIds = query.ProductIds
            .Where(id => products.All(p => p.Id != id))
            .ToList();

        if (missingProductIds.Count != 0)
        {
            return Result.Failure<List<Controllers.Product.GetProductResponse>>(
                $"Products not found: {string.Join(", ", missingProductIds)}");
        }

        var mappedProducts = products
            .Select(mapper.MapToResponse<Controllers.Product.GetProductResponse>)
            .ToList();

        return Result.Success(mappedProducts);
    }
}
