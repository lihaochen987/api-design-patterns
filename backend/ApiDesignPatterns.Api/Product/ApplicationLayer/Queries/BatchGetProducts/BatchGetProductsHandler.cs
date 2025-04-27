// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProducts;

public class BatchGetProductsHandler(IProductViewRepository repository, IMapper mapper)
    : IAsyncQueryHandler<BatchGetProductsQuery, List<Controllers.Product.GetProductResponse>>
{
    public async Task<List<Controllers.Product.GetProductResponse>> Handle(BatchGetProductsQuery query)
    {
        var products = await repository.GetProductsByIds(query.ProductIds);
        var mappedProducts = products.Select(productView => Enum.Parse<Category>(productView.Category) switch
            {
                Category.PetFood => mapper.Map<GetPetFoodResponse>(productView),
                Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(productView),
                _ => mapper.Map<Controllers.Product.GetProductResponse>(productView)
            })
            .ToList();
        return mappedProducts;
    }
}
