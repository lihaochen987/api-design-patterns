namespace backend.Contracts;

public class CreateProductRequest
{
    public string ProductName { get; set; } = "default";
    public string ProductPrice { get; set; } = "default";
    public string ProductCategory { get; set; } = "default";
}