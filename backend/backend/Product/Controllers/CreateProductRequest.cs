namespace backend.Product.Controllers;

public class CreateProductRequest
{
    public string Name { get; set; } = "";
    public string Price { get; set; } = "";
    public string Category { get; set; } = "";
}