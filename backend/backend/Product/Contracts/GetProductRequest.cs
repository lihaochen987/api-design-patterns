namespace backend.Product.Contracts;

public class GetProductRequest
{
    public string Id { get; set; } = "";
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}