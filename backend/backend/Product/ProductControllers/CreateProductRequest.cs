using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class CreateProductRequest
{
    public string Name { get; set; } = "";
    public string Price { get; set; } = "";
    public string Category { get; set; } = "";

    public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}