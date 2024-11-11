using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class CreateProductResponse
{
    public string Name { get; set; } = "";

    public ProductPricingContract Pricing { get; set; } =
        new() { BasePrice = "", DiscountPercentage = "", TaxRate = "" };

    public string Category { get; set; } = "";

    public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}