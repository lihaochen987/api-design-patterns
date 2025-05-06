// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Shared.QueryHandler;
using MapsterMapper;

namespace backend.Product.ApplicationLayer.Queries.MapListProductsResponse;

public class MapListProductsResponseHandler(IMapper mapper)
    : ISyncQueryHandler<MapListProductsResponseQuery, ListProductsResponse>
{
    public ListProductsResponse Handle(MapListProductsResponseQuery query)
    {
        IEnumerable<Controllers.Product.GetProductResponse> productResponses = query.PagedProducts.Products
            .Select(product =>
                Enum.Parse<Category>(product.Category) switch
                {
                    Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
                    Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
                    _ => mapper.Map<Controllers.Product.GetProductResponse>(product)
                }).ToList();
        ListProductsResponse response =
            new()
            {
                Results = productResponses,
                NextPageToken = query.PagedProducts.NextPageToken,
                TotalCount = query.PagedProducts.TotalCount
            };
        return response;
    }
}
