using System.Globalization;
using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class GetProductExtensions
{
    public GetProductResponse ToGetProductResponse(DomainModels.Product product)
    {
        return new GetProductResponse
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Price = product.BasePrice.ToString(CultureInfo.InvariantCulture),
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