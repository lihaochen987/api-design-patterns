using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Shared;

namespace backend.Product.ProductControllers;

public class ReplaceProductExtensions(TypeParser typeParser)
{
    public BaseProduct ToEntity(ReplaceProductRequest request)
    {
        // ProductPricing fields
        if (!decimal.TryParse(request.Pricing.DiscountPercentage, out var discountPercentage))
            throw new ArgumentException("Invalid discount percentage");
        if (!decimal.TryParse(request.Pricing.TaxRate, out var taxRate))
            throw new ArgumentException("Invalid tax rate");
        var basePrice = typeParser.ParseDecimal(request.Pricing.BasePrice, "Invalid BasePrice");

        // Product fields
        var category = typeParser.ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        var length = typeParser.ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        var width = typeParser.ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        var height = typeParser.ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        var dimensions = new Dimensions(length, width, height);
        var pricing = new Pricing(basePrice, discountPercentage, taxRate);
        return new DomainModels.BaseProduct(request.Name, pricing, category, dimensions);
    }

    public ReplaceProductResponse ToReplaceProductResponse(BaseProduct baseProduct)
    {
        return new ReplaceProductResponse
        {
            Name = baseProduct.Name,
            Category = baseProduct.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = baseProduct.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = baseProduct.Pricing.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
                TaxRate = baseProduct.Pricing.TaxRate.ToString(CultureInfo.InvariantCulture)
            },
            Dimensions = new DimensionsContract
            {
                Length = baseProduct.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = baseProduct.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = baseProduct.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }

    public ReplaceProductRequest ToReplaceProductRequest(BaseProduct baseProduct)
    {
        return new ReplaceProductRequest
        {
            Name = baseProduct.Name,
            Category = baseProduct.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = baseProduct.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = baseProduct.Pricing.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
                TaxRate = baseProduct.Pricing.TaxRate.ToString(CultureInfo.InvariantCulture)
            },
            Dimensions = new DimensionsContract
            {
                Length = baseProduct.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = baseProduct.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = baseProduct.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }
}