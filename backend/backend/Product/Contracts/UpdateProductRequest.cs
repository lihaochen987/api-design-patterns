namespace backend.Product.Contracts;

public class UpdateProductRequest : ProductContract
{
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}