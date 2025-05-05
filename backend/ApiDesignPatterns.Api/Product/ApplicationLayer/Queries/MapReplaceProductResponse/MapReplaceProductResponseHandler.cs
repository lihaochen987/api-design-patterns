// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;

public class MapReplaceProductResponseHandler(IProductTypeMapper mapper)
    : ISyncQueryHandler<MapReplaceProductResponseQuery, ReplaceProductResponse>
{
    public ReplaceProductResponse Handle(MapReplaceProductResponseQuery query)
    {
        var response = mapper.MapToResponse<ReplaceProductResponse>(query.Product);
        return response;
    }
}
