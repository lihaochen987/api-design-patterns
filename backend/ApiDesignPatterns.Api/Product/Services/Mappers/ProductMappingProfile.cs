// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.Services.Mappers;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Value Objects
        CreateMap<Pricing, ProductPricingResponse>().ReverseMap();
        CreateMap<Dimensions, DimensionsResponse>().ReverseMap();

        CreateMap<ProductPricingRequest, Pricing>().ReverseMap();
        CreateMap<DimensionsRequest, Dimensions>().ReverseMap();

        CreateMap<Name, string>().ConvertUsing(src => src.Value);
        CreateMap<string, Name>().ConvertUsing(src => new Name(src));

        CreateMap<Weight, decimal>().ConvertUsing(src => src.Value);
        CreateMap<decimal, Weight>().ConvertUsing(src => new Weight(src));

        // CreateProductController
        CreateMap<DomainModels.Product, CreateProductResponse>();
        CreateMap<PetFood, CreatePetFoodResponse>()
            .IncludeBase<DomainModels.Product, CreateProductResponse>();
        CreateMap<GroomingAndHygiene, CreateGroomingAndHygieneResponse>()
            .IncludeBase<DomainModels.Product, CreateProductResponse>();

        CreateMap<DomainModels.Product, CreateProductRequest>().ReverseMap();
        CreateMap<PetFood, CreateProductRequest>().ReverseMap();
        CreateMap<GroomingAndHygiene, CreateProductRequest>().ReverseMap();

        // GetProductController
        CreateMap<ProductView, GetProductResponse>();
        CreateMap<ProductView, GetPetFoodResponse>()
            .IncludeBase<ProductView, GetProductResponse>();
        CreateMap<ProductView, GetGroomingAndHygieneResponse>()
            .IncludeBase<ProductView, GetProductResponse>();

        // ReplaceProductController
        CreateMap<ReplaceProductRequest, DomainModels.Product>();
        CreateMap<ReplaceProductRequest, PetFood>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>();
        CreateMap<ReplaceProductRequest, GroomingAndHygiene>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>();

        CreateMap<DomainModels.Product, ReplaceProductResponse>();
        CreateMap<PetFood, ReplacePetFoodResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>();
        CreateMap<GroomingAndHygiene, ReplaceGroomingAndHygieneResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>();

        CreateMap<PetFood, ReplaceProductRequest>();
        CreateMap<DomainModels.Product, ReplaceProductRequest>();
        CreateMap<GroomingAndHygiene, ReplaceProductRequest>();

        // UpdateProductController
        CreateMap<DomainModels.Product, UpdateProductResponse>();
        CreateMap<PetFood, UpdatePetFoodResponse>()
            .IncludeBase<DomainModels.Product, UpdateProductResponse>();
        CreateMap<GroomingAndHygiene, UpdateGroomingAndHygieneResponse>()
            .IncludeBase<DomainModels.Product, UpdateProductResponse>();
    }
}
