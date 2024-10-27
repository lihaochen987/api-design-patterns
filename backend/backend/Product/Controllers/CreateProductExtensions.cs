using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using static backend.Shared.Parsers.TypeParser;

namespace backend.Product.Controllers;

public static class CreateProductExtensions
{
    public static DomainModels.Product ToEntity(this CreateProductRequest request)
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

    public static CreateProductResponse ToCreateProductResponse(this DomainModels.Product product)
    {
        return new CreateProductResponse
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