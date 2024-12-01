using System.Globalization;
using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels.Views;

namespace backend.Product.ProductControllers;

public class GetProductMappingProfile : Profile
{
    public GetProductMappingProfile()
    {
        CreateMap<ProductView, GetProductResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString(CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.Dimensions,
                opt => opt.MapFrom(src => new DimensionsContract
                {
                    Length = src.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
                    Width = src.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                    Height = src.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
                }));

        CreateMap<ProductView, GetPetFoodResponse>()
            .IncludeBase<ProductView, GetProductResponse>()
            .ForMember(dest => dest.AgeGroup,
                opt => opt.MapFrom(src => src.AgeGroup.ToString()))
            .ForMember(dest => dest.BreedSize,
                opt => opt.MapFrom(src => src.BreedSize.ToString()))
            .ForMember(dest => dest.NutritionalInfo,
                opt => opt.MapFrom(src =>
                    src.NutritionalInfo != null && src.NutritionalInfo.Count > 1 ? "Parsed Info" : null));

        CreateMap<ProductView, GetGroomingAndHygieneResponse>()
            .IncludeBase<ProductView, GetProductResponse>()
            .ForMember(dest => dest.IsNatural,
                opt => opt.MapFrom(src => src.IsNatural))
            .ForMember(dest => dest.IsHypoAllergenic,
                opt => opt.MapFrom(src => src.IsHypoallergenic))
            .ForMember(dest => dest.UsageInstructions,
                opt => opt.MapFrom(src => src.UsageInstructions))
            .ForMember(dest => dest.IsCrueltyFree,
                opt => opt.MapFrom(src => src.IsCrueltyFree))
            .ForMember(dest => dest.SafetyWarnings,
                opt => opt.MapFrom(src => src.SafetyWarnings));
    }
}
