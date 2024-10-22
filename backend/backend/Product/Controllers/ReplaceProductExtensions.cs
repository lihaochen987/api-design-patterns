using System.Globalization;

namespace backend.Product.Controllers;

public static class ReplaceProductExtensions
{
    public static Product ToEntity(this ReplaceProductRequest request)
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

    public static ReplaceProductResponse ToReplaceProductResponse(this Product product)
    {
        return new ReplaceProductResponse
        {
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture),
            Category = product.Category.ToString()
        };
    }
}