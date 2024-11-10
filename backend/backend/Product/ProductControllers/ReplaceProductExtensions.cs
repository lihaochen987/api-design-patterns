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
        if (!DiscountPercentage.TryParse(request.DiscountPercentage, out var discountPercentage) ||
            discountPercentage is null)
            throw new ArgumentException("Invalid discount percentage");
        if (!TaxRate.TryParse(request.TaxRate, out var taxRate) ||
            taxRate is null)
            throw new ArgumentException("Invalid tax rate");
        var category = typeParser.ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        var length = typeParser.ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        var width = typeParser.ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        var height = typeParser.ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        var dimensions = new Dimensions(length, width, height);
        return new DomainModels.Product(request.Name, category, dimensions);
    }

    public ReplaceProductResponse ToReplaceProductResponse(DomainModels.Product product)
    {
        return new ReplaceProductResponse
        {
            Name = product.Name,
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