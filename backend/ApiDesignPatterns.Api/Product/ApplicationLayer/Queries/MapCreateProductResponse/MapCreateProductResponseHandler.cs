// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;

public class MapCreateProductResponseHandler(
    IProductTypeMapper mapper)
    : ISyncQueryHandler<MapCreateProductResponseQuery, CreateProductResponse>
{
    public CreateProductResponse Handle(MapCreateProductResponseQuery query)
    {
        var response = mapper.MapToResponse<CreateProductResponse>(query.Product);
        return response;
    }
}
