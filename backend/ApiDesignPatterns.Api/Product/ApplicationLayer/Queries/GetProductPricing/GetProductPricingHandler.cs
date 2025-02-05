// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer.Database.ProductPricing;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetProductPricing;

public class GetProductPricingHandler(IProductPricingRepository repository)
    : IQueryHandler<GetProductPricingQuery, ProductPricingView>
{
    public async Task<ProductPricingView?> Handle(GetProductPricingQuery query)
    {
        ProductPricingView? productPricing = await repository.GetProductPricingAsync(query.Id);
        return productPricing ?? null;
    }
}
