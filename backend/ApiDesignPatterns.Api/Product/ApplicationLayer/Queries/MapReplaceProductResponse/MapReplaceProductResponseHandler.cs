// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapReplaceProductResponse;

public class MapReplaceProductResponseHandler(IMapper mapper)
    : IQueryHandler<MapReplaceProductResponseQuery, ReplaceProductResponse>
{
    public Task<ReplaceProductResponse?> Handle(MapReplaceProductResponseQuery query)
    {
        ReplaceProductResponse response = query.Product.Category switch
        {
            Category.PetFood => mapper.Map<ReplacePetFoodResponse>(query.Product),
            Category.GroomingAndHygiene => mapper.Map<ReplaceGroomingAndHygieneResponse>(query.Product),
            _ => mapper.Map<ReplaceProductResponse>(query.Product)
        };
        return Task.FromResult(response)!;
    }
}
