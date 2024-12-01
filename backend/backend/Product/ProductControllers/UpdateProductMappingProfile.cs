using System.Globalization;
using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels;

namespace backend.Product.ProductControllers;

public class UpdateProductMappingProfile : Profile
{
    public UpdateProductMappingProfile()
    {
        CreateMap<DomainModels.Product, UpdateProductResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Pricing,
                opt => opt.MapFrom(src => new ProductPricingContract
                {
                    BasePrice = src.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                    DiscountPercentage = src.Pricing.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
                    TaxRate = src.Pricing.TaxRate.ToString(CultureInfo.InvariantCulture)
                }))
            .ForMember(dest => dest.Dimensions,
                opt => opt.MapFrom(src => new DimensionsContract
                {
                    Length = src.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
                    Width = src.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                    Height = src.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
                }));

        CreateMap<PetFood, UpdatePetFoodResponse>()
            .IncludeBase<DomainModels.Product, UpdateProductResponse>()
            .ForMember(dest => dest.AgeGroup, opt => opt.MapFrom(src => src.AgeGroup.ToString()))
            .ForMember(dest => dest.BreedSize, opt => opt.MapFrom(src => src.BreedSize.ToString()))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.NutritionalInfo,
                opt => opt.MapFrom(src =>
                    src.NutritionalInfo.Count > 1 ? "Parsed Info" : null))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => src.StorageInstructions))
            .ForMember(dest => dest.WeightKg,
                opt => opt.MapFrom(src => src.WeightKg.ToString(CultureInfo.InvariantCulture)));

        CreateMap<GroomingAndHygiene, UpdateGroomingAndHygieneResponse>()
            .IncludeBase<DomainModels.Product, UpdateProductResponse>()
            .ForMember(dest => dest.IsNatural, opt => opt.MapFrom(src => src.IsNatural))
            .ForMember(dest => dest.IsHypoAllergenic, opt => opt.MapFrom(src => src.IsHypoallergenic))
            .ForMember(dest => dest.UsageInstructions, opt => opt.MapFrom(src => src.UsageInstructions))
            .ForMember(dest => dest.IsCrueltyFree, opt => opt.MapFrom(src => src.IsCrueltyFree))
            .ForMember(dest => dest.SafetyWarnings, opt => opt.MapFrom(src => src.SafetyWarnings));
    }
}
