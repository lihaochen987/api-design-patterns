// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Shared;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProducts;

public class BatchGetProductsHandler(IProductViewRepository repository, IMapper mapper)
    : IAsyncQueryHandler<BatchGetProductsQuery, Result<List<Controllers.Product.GetProductResponse>>>
{
    public async Task<Result<List<Controllers.Product.GetProductResponse>>> Handle(BatchGetProductsQuery query)
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

        var mappedProducts = products.Select(productView => Enum.Parse<Category>(productView.Category) switch
            {
                Category.PetFood => mapper.Map<GetPetFoodResponse>(productView),
                Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(productView),
                _ => mapper.Map<Controllers.Product.GetProductResponse>(productView)
            })
            .ToList();

        return Result.Success(mappedProducts);
    }
}
