// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Shared.QueryHandler;
using MapsterMapper;

namespace backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;

public class MapCreateProductRequestHandler(IMapper mapper)
    : ISyncQueryHandler<MapCreateProductRequestQuery, DomainModels.Product>
{
    public DomainModels.Product Handle(MapCreateProductRequestQuery query)
    {
        DomainModels.Product product = query.Request.Category switch
        {
            nameof(Category.PetFood) => mapper.Map<PetFood>(query.Request),
            nameof(Category.GroomingAndHygiene) => mapper.Map<GroomingAndHygiene>(query.Request),
            _ => mapper.Map<DomainModels.Product>(query.Request)
        };

        return product;
    }
}
