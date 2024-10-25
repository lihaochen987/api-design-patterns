using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using static backend.Parsers.ParseDecimalUtility;
using static backend.Parsers.ParseEnumUtility;

namespace backend.Product.Controllers;

public static class ReplaceProductExtensions
{
    public static DomainModels.Product ToEntity(this ReplaceProductRequest request)
    {
        // Product Fields
        var price = ParseDecimal(request.Price, "Invalid product price");
        var category = ParseEnum<Category>(request.Category, "Invalid product category");

        // Dimensions Fields
        var length = ParseDecimal(request.Dimensions.Length, "Invalid dimensions length");
        var width = ParseDecimal(request.Dimensions.Width, "Invalid dimensions width");
        var height = ParseDecimal(request.Dimensions.Height, "Invalid dimensions height");

        var dimensions = new Dimensions(length, width, height);
        return new DomainModels.Product(request.Name, price, category, dimensions);
    }

    public static ReplaceProductResponse ToReplaceProductResponse(this DomainModels.Product product)
    {
        return new ReplaceProductResponse
        {
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture),
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