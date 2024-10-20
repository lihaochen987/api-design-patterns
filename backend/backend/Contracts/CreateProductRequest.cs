namespace backend.Contracts;

public class CreateProductRequest
{
    public string ProductName { get; set; } = "";
    public string ProductPrice { get; set; } = "";
    public string ProductCategory { get; set; } = "";
}