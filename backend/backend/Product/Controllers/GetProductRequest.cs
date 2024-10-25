namespace backend.Product.Controllers;

public class GetProductRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}