// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductView;
using backend.Product.Services.Mappers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetProductResponse;

public class GetProductResponseHandler(
    IProductViewRepository repository,
    IProductTypeMapper mapper)
    : IAsyncQueryHandler<GetProductResponseQuery, Controllers.Product.GetProductResponse?>
{
    public async Task<Controllers.Product.GetProductResponse?> Handle(GetProductResponseQuery query)
    {
        ProductView? productView = await repository.GetProductView(query.Id);

        if (productView == null)
        {
            return null;
        }

        var response = mapper.MapToResponse<Controllers.Product.GetProductResponse>(productView);

        return response;
    }
}
