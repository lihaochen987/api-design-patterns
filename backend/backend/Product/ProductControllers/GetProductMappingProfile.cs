using System.Globalization;
using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.DomainModels.Views;

namespace backend.Product.ProductControllers;

public class GetProductMappingProfile : Profile
{
    public GetProductMappingProfile()
    {
        CreateMap<Pricing, ProductPricingContract>();
        CreateMap<Dimensions, DimensionsContract>();
        CreateMap<ProductView, GetProductResponse>();

        CreateMap<ProductView, GetPetFoodResponse>()
            .IncludeBase<ProductView, GetProductResponse>();

        CreateMap<ProductView, GetGroomingAndHygieneResponse>()
            .IncludeBase<ProductView, GetProductResponse>();
    }
}
