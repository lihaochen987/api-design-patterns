namespace backend.Product.Controllers.Product;

public record GetProductRequest
{
    public List<string> FieldMask { get; init; } = ["*"];
}
