using System.Globalization;

namespace backend.Product.Controllers;

public static class CreateProductExtensions
{
    public static Product ToEntity(this CreateProductRequest request)
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

    public static CreateProductResponse ToCreateProductResponse(this Product product)
    {
        return new CreateProductResponse
        {
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture),
            Category = product.Category.ToString()
        };
    }
}