namespace backend.Contracts;

public class UpdateProductRequest
{
    public string ProductName { get; set; } = "";
    public string ProductPrice { get; set; } = "";
    public string ProductCategory { get; set; } = "";
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}