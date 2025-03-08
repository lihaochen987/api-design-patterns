// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapListProductsResponse;

public class MapListProductsResponseHandler(IMapper mapper)
    : IQueryHandler<MapListProductsResponseQuery, ListProductsResponse>
{
    public Task<ListProductsResponse?> Handle(MapListProductsResponseQuery query)
    {
        IEnumerable<ProductControllers.GetProductResponse> productResponses = query.PagedProducts.Products.Select(
            product =>
                Enum.Parse<Category>(product.Category) switch
                {
                    Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
                    Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
                    _ => mapper.Map<ProductControllers.GetProductResponse>(product)
                }).ToList();
        ListProductsResponse response =
            new() { Results = productResponses, NextPageToken = query.PagedProducts.NextPageToken };
        return Task.FromResult(response)!;
    }
}
