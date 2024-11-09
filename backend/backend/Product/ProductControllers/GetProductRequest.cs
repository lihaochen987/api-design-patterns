namespace backend.Product.ProductControllers;

public class GetProductRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}