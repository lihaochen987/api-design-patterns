// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Mapster;
using backend.Product.Controllers.Product;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.Services.Mappers;

public static class ProductMappingConfig
{
    public static void RegisterProductMappings(this TypeAdapterConfig config)
    {
        // Value Objects
        config.NewConfig<Pricing, ProductPricingResponse>().TwoWays();
        config.NewConfig<Dimensions, DimensionsResponse>().TwoWays();

        config.NewConfig<ProductPricingRequest, Pricing>().TwoWays();
        config.NewConfig<DimensionsRequest, Dimensions>().TwoWays();

        config.NewConfig<Name, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, Name>()
            .MapWith(src => new Name(src));

        config.NewConfig<Weight, decimal?>()
            .MapWith(src => src.Value);
        config.NewConfig<decimal?, Weight>()
            .MapWith(src => new Weight(src));

        config.NewConfig<UsageInstructions, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, UsageInstructions>()
            .MapWith(src => new UsageInstructions(src));

        config.NewConfig<SafetyWarnings, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, SafetyWarnings>()
            .MapWith(src => new SafetyWarnings(src));

        config.NewConfig<Ingredients, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, Ingredients>()
            .MapWith(src => new Ingredients(src));

        config.NewConfig<StorageInstructions, string>()
            .MapWith(src => src.Value);
        config.NewConfig<string, StorageInstructions>()
            .MapWith(src => new StorageInstructions(src));

        // CreateProductController
        config.NewConfig<DomainModels.Product, CreateProductResponse>();
        config.NewConfig<PetFood, CreatePetFoodResponse>()
            .Inherits<DomainModels.Product, CreateProductResponse>();
        config.NewConfig<GroomingAndHygiene, CreateGroomingAndHygieneResponse>()
            .Inherits<DomainModels.Product, CreateProductResponse>();

        config.NewConfig<DomainModels.Product, CreateProductRequest>().TwoWays();
        config.NewConfig<PetFood, CreateProductRequest>().TwoWays();
        config.NewConfig<GroomingAndHygiene, CreateProductRequest>().TwoWays();

        // GetProductController
        config.NewConfig<ProductView, GetProductResponse>();
        config.NewConfig<ProductView, GetPetFoodResponse>()
            .Inherits<ProductView, GetProductResponse>();
        config.NewConfig<ProductView, GetGroomingAndHygieneResponse>()
            .Inherits<ProductView, GetProductResponse>();

        // ReplaceProductController
        config.NewConfig<ReplaceProductRequest, DomainModels.Product>();
        config.NewConfig<ReplaceProductRequest, PetFood>()
            .Inherits<ReplaceProductRequest, DomainModels.Product>();
        config.NewConfig<ReplaceProductRequest, GroomingAndHygiene>()
            .Inherits<ReplaceProductRequest, DomainModels.Product>();

        config.NewConfig<DomainModels.Product, ReplaceProductResponse>();
        config.NewConfig<PetFood, ReplacePetFoodResponse>()
            .Inherits<DomainModels.Product, ReplaceProductResponse>();
        config.NewConfig<GroomingAndHygiene, ReplaceGroomingAndHygieneResponse>()
            .Inherits<DomainModels.Product, ReplaceProductResponse>();

        config.NewConfig<PetFood, ReplaceProductRequest>();
        config.NewConfig<DomainModels.Product, ReplaceProductRequest>();
        config.NewConfig<GroomingAndHygiene, ReplaceProductRequest>();

        // UpdateProductController
        config.NewConfig<DomainModels.Product, UpdateProductResponse>();
        config.NewConfig<PetFood, UpdatePetFoodResponse>()
            .Inherits<DomainModels.Product, UpdateProductResponse>();
        config.NewConfig<GroomingAndHygiene, UpdateGroomingAndHygieneResponse>()
            .Inherits<DomainModels.Product, UpdateProductResponse>();
    }
}
