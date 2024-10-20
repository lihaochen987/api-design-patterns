namespace backend.Contracts;

public class UpdateProductRequest : ProductContract
{
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}