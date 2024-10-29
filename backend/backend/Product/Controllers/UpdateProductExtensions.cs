using System.Globalization;
using backend.Product.Contracts;

namespace backend.Product.Controllers;

public class UpdateProductExtensions
{
    public UpdateProductResponse ToUpdateProductResponse(DomainModels.Product product)
    {
        return new UpdateProductResponse
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Price = product.Price.ToString(CultureInfo.InvariantCulture),
            Category = product.Category.ToString(),
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Length.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }
}