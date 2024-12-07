using System.Globalization;
using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductControllers;

public class UpdateProductMappingProfile : Profile
{
    public UpdateProductMappingProfile()
    {
        CreateMap<Pricing, ProductPricingContract>().ReverseMap();
        CreateMap<Dimensions, DimensionsContract>().ReverseMap();

        CreateMap<DomainModels.Product, UpdateProductResponse>();
        CreateMap<PetFood, UpdatePetFoodResponse>()
            .IncludeBase<DomainModels.Product, UpdateProductResponse>();
        CreateMap<GroomingAndHygiene, UpdateGroomingAndHygieneResponse>();
    }
}
