namespace backend.Product.Controllers;

public class UpdateProductRequest
{
    public string Name { get; set; } = "";
    public string Price { get; set; } = "";
    public string Category { get; set; } = "";
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}