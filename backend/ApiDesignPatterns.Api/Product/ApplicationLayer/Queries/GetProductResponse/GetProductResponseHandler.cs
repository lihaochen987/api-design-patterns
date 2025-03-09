// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.ProductControllers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetProductResponse;

public class GetProductResponseHandler(
    IProductViewRepository repository,
    IMapper mapper)
    : IQueryHandler<GetProductResponseQuery, ProductControllers.GetProductResponse?>
{
    public async Task<ProductControllers.GetProductResponse?> Handle(GetProductResponseQuery query)
    {
        ProductView? productView = await repository.GetProductView(query.Id);

        if (productView == null)
        {
            return null;
        }

        ProductControllers.GetProductResponse response = Enum.Parse<Category>(productView.Category) switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(productView),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(productView),
            _ => mapper.Map<ProductControllers.GetProductResponse>(productView)
        };

        return response;
    }
}
