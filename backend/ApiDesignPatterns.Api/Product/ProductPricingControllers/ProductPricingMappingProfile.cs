// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.ProductPricingControllers;

public class ProductPricingMappingProfile : Profile
{
    public ProductPricingMappingProfile()
    {
        // GetProductPricingController
        CreateMap<Pricing, ProductPricingResponse>();
        CreateMap<ProductPricingView, GetProductPricingResponse>();
        CreateMap<ProductPricingView, ProductPricingResponse>();

        CreateMap<PetFood, UpdateProductPricingResponse>();
    }
}
