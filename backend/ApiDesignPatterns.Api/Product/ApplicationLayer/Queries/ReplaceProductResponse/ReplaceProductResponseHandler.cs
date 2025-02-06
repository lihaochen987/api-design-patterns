// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels.Enums;
using backend.Product.ProductControllers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.ReplaceProductResponse;

public class
    ReplaceProductResponseHandler(IMapper mapper)
    : IQueryHandler<ReplaceProductResponseQuery, ProductControllers.ReplaceProductResponse>
{
    public Task<ProductControllers.ReplaceProductResponse?> Handle(ReplaceProductResponseQuery query)
    {
        ProductControllers.ReplaceProductResponse response = query.Product.Category switch
        {
            Category.PetFood => mapper.Map<ReplacePetFoodResponse>(query.Product),
            Category.GroomingAndHygiene => mapper.Map<ReplaceGroomingAndHygieneResponse>(query.Product),
            _ => mapper.Map<ProductControllers.ReplaceProductResponse>(query.Product)
        };
        return Task.FromResult(response)!;
    }
}
