using backend.Product.Contracts;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductControllers;

using AutoMapper;
using DomainModels;

public class CreateProductMappingProfile : Profile
{
    public CreateProductMappingProfile()
    {
        CreateMap<Pricing, ProductPricingContract>();
        CreateMap<Dimensions, DimensionsContract>();
        CreateMap<Product, CreateProductResponse>();
        CreateMap<PetFood, CreatePetFoodResponse>()
            .IncludeBase<Product, CreateProductResponse>();
        CreateMap<GroomingAndHygiene, CreateGroomingAndHygieneResponse>()
            .IncludeBase<Product, CreateProductResponse>();
    }
}
