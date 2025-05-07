// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Mapster;
using backend.Product.Controllers.ProductPricing;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.Services.Mappers;

public static class ProductPricingMappingConfig
{
    public static void RegisterProductPricingMappings(this TypeAdapterConfig config)
    {
        // GetProductPricingController
        config.NewConfig<Pricing, ProductPricingResponse>();

        config.NewConfig<ProductPricingView, GetProductPricingResponse>();
        config.NewConfig<ProductPricingView, ProductPricingResponse>();

        config.NewConfig<PetFood, UpdateProductPricingResponse>();
    }
}
