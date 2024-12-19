namespace backend.Product.ProductControllers;

public record GetProductRequest
{
    public List<string> FieldMask { get; init; } = ["*"];
}
