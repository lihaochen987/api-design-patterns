namespace backend.Contracts;

public class UpdateProductRequest
{
    public string ProductName { get; set; } = "default";
    public string ProductPrice { get; set; } = "default";
    public string ProductCategory { get; set; } = "default";
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}