using System.Globalization;
using backend.Product.Contracts;
using backend.Product.DomainModels;
using backend.Shared;
using ArgumentException = System.ArgumentException;

namespace backend.Product.ProductControllers;

public class CreateProductExtensions(TypeParser typeParser)
{
    public DomainModels.Product ToEntity(CreateProductRequest request)
    {
        // Product Fields
        var basePrice = typeParser.ParseDecimal(request.BasePrice, "Invalid base price");
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
        return new DomainModels.Product(request.Name, basePrice, discountPercentage, taxRate, category, dimensions);
    }

    public CreateProductResponse ToCreateProductResponse(DomainModels.Product product)
    {
        return new CreateProductResponse
        {
            Name = product.Name,
            BasePrice = product.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = product.DiscountPercentage.ToString(),
            TaxRate = product.TaxRate.ToString(),
            Category = product.Category.ToString(),
            Dimensions = new DimensionsContract
            {
                Length = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Width = product.Dimensions.Width.ToString(CultureInfo.InvariantCulture),
                Height = product.Dimensions.Height.ToString(CultureInfo.InvariantCulture)
            }
        };
    }

    public CreateProductRequest ToCreateProductRequest(DomainModels.Product product)
    {
        return new CreateProductRequest
        {
            Name = product.Name,
            BasePrice = product.BasePrice.ToString(CultureInfo.InvariantCulture),
            DiscountPercentage = product.DiscountPercentage.ToString(),
            TaxRate = product.TaxRate.ToString(),
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