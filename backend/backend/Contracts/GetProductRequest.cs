namespace backend.Contracts;

public class GetProductRequest
{
    public string Id { get; set; } = "default";
    public IEnumerable<string> FieldMask { get; init; } = ["*"];
}