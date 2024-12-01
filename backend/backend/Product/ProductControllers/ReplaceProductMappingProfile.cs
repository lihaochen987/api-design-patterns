using System.Globalization;
using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;

namespace backend.Product.ProductControllers;

public class ReplaceProductMappingProfile : Profile
{
    public ReplaceProductMappingProfile()
    {
        CreateMap<ReplaceProductRequest, DomainModels.Product>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Pricing, opt => opt.MapFrom(src =>
                new Pricing(
                    decimal.Parse(src.Pricing.BasePrice, CultureInfo.InvariantCulture),
                    decimal.Parse(src.Pricing.DiscountPercentage, CultureInfo.InvariantCulture),
                    decimal.Parse(src.Pricing.TaxRate, CultureInfo.InvariantCulture))))
            .ForMember(dest => dest.Dimensions, opt => opt.MapFrom(src =>
                new Dimensions(
                    decimal.Parse(src.Dimensions.Length, CultureInfo.InvariantCulture),
                    decimal.Parse(src.Dimensions.Width, CultureInfo.InvariantCulture),
                    decimal.Parse(src.Dimensions.Height, CultureInfo.InvariantCulture))));

        CreateMap<ReplaceProductRequest, PetFood>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>()
            .ForMember(dest => dest.AgeGroup, opt => opt.MapFrom(src => Enum.Parse<AgeGroup>(src.AgeGroup ??
                string.Empty)))
            .ForMember(dest => dest.BreedSize, opt => opt.MapFrom(src => Enum.Parse<BreedSize>(src.BreedSize ??
                string.Empty)))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.NutritionalInfo, opt => opt.MapFrom(src => src.NutritionalInfo))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => src.StorageInstructions))
            .ForMember(dest => dest.WeightKg,
                opt => opt.MapFrom(src => decimal.Parse(src.WeightKg ?? string.Empty, CultureInfo.InvariantCulture)));

        CreateMap<ReplaceProductRequest, GroomingAndHygiene>()
            .IncludeBase<ReplaceProductRequest, DomainModels.Product>()
            .ForMember(dest => dest.IsNatural, opt => opt.MapFrom(src => src.IsNatural))
            .ForMember(dest => dest.IsHypoallergenic, opt => opt.MapFrom(src => src.IsHypoAllergenic))
            .ForMember(dest => dest.UsageInstructions, opt => opt.MapFrom(src => src.UsageInstructions))
            .ForMember(dest => dest.IsCrueltyFree, opt => opt.MapFrom(src => src.IsCrueltyFree))
            .ForMember(dest => dest.SafetyWarnings, opt => opt.MapFrom(src => src.SafetyWarnings));

        CreateMap<DomainModels.Product, ReplaceProductResponse>()
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

        CreateMap<PetFood, ReplacePetFoodResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>()
            .ForMember(dest => dest.AgeGroup, opt => opt.MapFrom(src => src.AgeGroup.ToString()))
            .ForMember(dest => dest.BreedSize, opt => opt.MapFrom(src => src.BreedSize.ToString()))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.NutritionalInfo,
                opt => opt.MapFrom(src =>
                    src.NutritionalInfo.Count > 1 ? "Parsed Info" : null))
            .ForMember(dest => dest.StorageInstructions, opt => opt.MapFrom(src => src.StorageInstructions))
            .ForMember(dest => dest.WeightKg,
                opt => opt.MapFrom(src => src.WeightKg.ToString(CultureInfo.InvariantCulture)));

        CreateMap<GroomingAndHygiene, ReplaceGroomingAndHygieneResponse>()
            .IncludeBase<DomainModels.Product, ReplaceProductResponse>()
            .ForMember(dest => dest.IsNatural, opt => opt.MapFrom(src => src.IsNatural))
            .ForMember(dest => dest.IsHypoAllergenic, opt => opt.MapFrom(src => src.IsHypoallergenic))
            .ForMember(dest => dest.UsageInstructions, opt => opt.MapFrom(src => src.UsageInstructions))
            .ForMember(dest => dest.IsCrueltyFree, opt => opt.MapFrom(src => src.IsCrueltyFree))
            .ForMember(dest => dest.SafetyWarnings, opt => opt.MapFrom(src => src.SafetyWarnings));
    }
}
