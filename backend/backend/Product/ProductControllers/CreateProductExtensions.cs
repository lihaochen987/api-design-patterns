using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Shared;
using ArgumentException = System.ArgumentException;

namespace backend.Product.ProductControllers;

public class CreateProductExtensions(TypeParser typeParser)
{
    public DomainModels.Product ToEntity(CreateProductRequest request)
    {
        // ProductPricing fields
        if (!DiscountPercentage.TryParse(request.Pricing.DiscountPercentage, out var discountPercentage) ||
            discountPercentage is null)
            throw new ArgumentException("Invalid discount percentage");
        if (!TaxRate.TryParse(request.Pricing.TaxRate, out var taxRate) ||
            taxRate is null)
            throw new ArgumentException("Invalid tax rate");
        var basePrice = typeParser.ParseDecimal(request.Pricing.BasePrice, "Invalid BasePrice");

        // Product fields
        var category = typeParser.ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        var length = typeParser.ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        var width = typeParser.ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        var height = typeParser.ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        var dimensions = new Dimensions(length, width, height);
        var pricing = new ProductPricing(basePrice, discountPercentage, taxRate);
        return new DomainModels.Product(request.Name, pricing, category, dimensions);
    }

    public CreateProductResponse ToCreateProductResponse(DomainModels.Product product)
    {
        return new CreateProductResponse
        {
            Name = product.Name,
            Category = product.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = product.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = product.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                TaxRate = product.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
            },
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }

    public CreateProductRequest ToCreateProductRequest(DomainModels.Product product)
    {
        return new CreateProductRequest
        {
            Name = product.Name,
            Category = product.Category.ToString(),
            Pricing = new ProductPricingContract
            {
                BasePrice = product.Pricing.BasePrice.ToString(CultureInfo.InvariantCulture),
                DiscountPercentage = product.Pricing.DiscountPercentage.ToString(),
                TaxRate = product.Pricing.TaxRate.ToString()
            },
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }
}