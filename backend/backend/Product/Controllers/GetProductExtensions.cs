using System.Globalization;

namespace backend.Product.Controllers;

public static class GetProductExtensions
{
    public static GetProductResponse ToGetProductResponse(this Product product)
    {
        return new GetProductResponse
        {
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture),
            Category = product.Category.ToString()
        };
    }
}