using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class CreateProductResponse
{
    public string Name { get; set; } = "";
    public string BasePrice { get; set; } = "";
    public string DiscountPercentage { get; set; } = "";
    public string TaxRate { get; set; } = "";
    public string Category { get; set; } = "";

    public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}