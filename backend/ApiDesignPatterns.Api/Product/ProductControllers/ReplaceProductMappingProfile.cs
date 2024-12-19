using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductControllers;

public class ReplaceProductMappingProfile : Profile
{
    public ReplaceProductMappingProfile()
    {
        // Map Requests
        CreateMap<ProductPricingRequest, Pricing>().ReverseMap();
        CreateMap<DimensionsRequest, Dimensions>().ReverseMap();

        CreateMap<ReplaceProductRequest, DomainModels.Product>();
        CreateMap<ReplaceProductRequest, PetFood>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>();
        CreateMap<ReplaceProductRequest, GroomingAndHygiene>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>();

        // Map Responses
        CreateMap<Pricing, ProductPricingResponse>().ReverseMap();
        CreateMap<Dimensions, DimensionsResponse>().ReverseMap();

        CreateMap<DomainModels.Product, ReplaceProductResponse>();
        CreateMap<PetFood, ReplacePetFoodResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>();
        CreateMap<GroomingAndHygiene, ReplaceGroomingAndHygieneResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>();
    }
}
