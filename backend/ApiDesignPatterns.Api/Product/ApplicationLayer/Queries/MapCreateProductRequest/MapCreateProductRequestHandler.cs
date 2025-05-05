// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;

public class MapCreateProductRequestHandler(IProductTypeMapper mapper)
    : ISyncQueryHandler<MapCreateProductRequestQuery, DomainModels.Product>
{
    public DomainModels.Product Handle(MapCreateProductRequestQuery query)
    {
        var product = mapper.MapFromRequest(query.Request);
        return product;
    }
}
