using backend.Product.Contracts;
using backend.Product.DomainModels;

namespace backend.Product.Controllers;

public class CreateProductResponse
{
    public string Name { get; set; } = "";
    public string Price { get; set; } = "";
    public string Category { get; set; } = "";

    public DimensionsContract Dimensions { get; set; } = new() { Length = "", Width = "", Height = "" };
}