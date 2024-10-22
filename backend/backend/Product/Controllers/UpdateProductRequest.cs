namespace backend.Product.Controllers;

public class UpdateProductRequest : ProductContract
{
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}