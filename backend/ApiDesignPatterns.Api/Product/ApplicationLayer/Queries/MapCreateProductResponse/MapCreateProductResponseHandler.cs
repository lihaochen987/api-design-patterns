// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;

public class MapCreateProductResponseHandler(
    IMapper mapper)
    : ISyncQueryHandler<MapCreateProductResponseQuery, CreateProductResponse>
{
    public CreateProductResponse Handle(MapCreateProductResponseQuery query)
    {
        CreateProductResponse response = query.Product.Category switch
        {
            Category.PetFood => mapper.Map<CreatePetFoodResponse>(query.Product),
            Category.GroomingAndHygiene => mapper.Map<CreateGroomingAndHygieneResponse>(query.Product),
            _ => mapper.Map<CreateProductResponse>(query.Product)
        };
        return response;
    }
}
