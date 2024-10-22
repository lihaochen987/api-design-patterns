using System.Globalization;
using backend.Product.Controllers;

namespace backend.Product;

public class ProductMapper
{
    public static Product MapToDomain(CreateProductRequest request)
    {
        if (!decimal.TryParse(request.Price, out var price))
        {
            throw new ArgumentException("Invalid product price");
        }

        if (!Enum.TryParse<Category>(request.Category, out var category))
        {
            throw new ArgumentException("Invalid product category");
        }

        return new Product(request.Name, price, category);
    }

    public static Product MapToDomain(ReplaceProductRequest request)
    {
        if (!decimal.TryParse(request.Price, out var price))
        {
            throw new ArgumentException("Invalid product price");
        }

        if (!Enum.TryParse<Category>(request.Category, out var category))
        {
            throw new ArgumentException("Invalid product category");
        }

        return new Product(request.Name, price, category);
    }

    public static CreateProductResponse MapToCreateProductResponse(Product product)
    {
        return new CreateProductResponse
        {
            Category = product.Category.ToString(),
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture)
        };
    }

    public static GetProductResponse MapToGetProductResponse(Product product)
    {
        return new GetProductResponse
        {
            Category = product.Category.ToString(),
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture)
        };
    }

    public static ReplaceProductResponse MapToReplaceProductResponse(Product product)
    {
        return new ReplaceProductResponse
        {
            Category = product.Category.ToString(),
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture)
        };
    }
}