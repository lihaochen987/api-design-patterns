using backend.Product.Contracts;

namespace backend.Product.ProductControllers;

public class GetProductResponse
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Price { get; set; } = "";
    public string Category { get; set; } = "";
    public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}