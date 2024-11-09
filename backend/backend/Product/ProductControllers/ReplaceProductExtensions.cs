using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Shared;

namespace backend.Product.ProductControllers;

public class ReplaceProductExtensions(TypeParser typeParser)
{
    public DomainModels.Product ToEntity(ReplaceProductRequest request)
    {
        // Product Fields
        var basePrice = typeParser.ParseDecimal(request.BasePrice, "Invalid base price");
        var discountPercentage = typeParser.ParseDecimal(request.DiscountPercentage, "Invalid discount percentage");
        var taxRate = typeParser.ParseDecimal(request.TaxRate, "Invalid tax rate");
        var category = typeParser.ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        var length = typeParser.ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        var width = typeParser.ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        var height = typeParser.ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        var dimensions = new Dimensions(length, width, height);
        return new DomainModels.Product(request.Name, basePrice, discountPercentage, taxRate, category, dimensions);
    }

    public ReplaceProductResponse ToReplaceProductResponse(DomainModels.Product product)
    {
        return new ReplaceProductResponse
        {
            Name = product.Name,
            BasePrice = product.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = product.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
            TaxRate = product.TaxRate.ToString(CultureInfo.InvariantCulture),
            Category = product.Category.ToString(),
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }

    public ReplaceProductRequest ToReplaceProductRequest(DomainModels.Product product)
    {
        return new ReplaceProductRequest
        {
            Name = product.Name,
            BasePrice = product.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = product.DiscountPercentage.ToString(CultureInfo.InvariantCulture),
            TaxRate = product.TaxRate.ToString(CultureInfo.InvariantCulture),
            Category = product.Category.ToString(),
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }
}