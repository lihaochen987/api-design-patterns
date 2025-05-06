// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Shared.QueryHandler;
using MapsterMapper;

namespace backend.Product.ApplicationLayer.Queries.GetProductResponse;

public class GetProductResponseHandler(
    IProductViewRepository repository,
    IMapper mapper)
    : IAsyncQueryHandler<GetProductResponseQuery, Controllers.Product.GetProductResponse?>
{
    public async Task<Controllers.Product.GetProductResponse?> Handle(GetProductResponseQuery query)
    {
        ProductView? productView = await repository.GetProductView(query.Id);

        if (productView == null)
        {
            return null;
        }

        Controllers.Product.GetProductResponse response = Enum.Parse<Category>(productView.Category) switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(productView),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(productView),
            _ => mapper.Map<Controllers.Product.GetProductResponse>(productView)
        };

        return response;
    }
}
