using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductControllers;

public class ReplaceProductMappingProfile : Profile
{
    public ReplaceProductMappingProfile()
    {
        CreateMap<Pricing, ProductPricingContract>().ReverseMap();
        CreateMap<Dimensions, DimensionsContract>().ReverseMap();

        // Map Requests
        CreateMap<ReplaceProductRequest, DomainModels.Product>();
        CreateMap<ReplaceProductRequest, PetFood>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>();
        CreateMap<ReplaceProductRequest, GroomingAndHygiene>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>();

        // Map Responses
        CreateMap<DomainModels.Product, ReplaceProductResponse>();
        CreateMap<PetFood, ReplacePetFoodResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>();
        CreateMap<GroomingAndHygiene, ReplaceGroomingAndHygieneResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>();
    }
}
