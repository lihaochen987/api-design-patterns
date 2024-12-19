namespace backend.Product.DomainModels.ValueObjects;

public class DimensionsResponse
{
    public required string Length { get; init; }
    public required string Width { get; init; }
    public required string Height { get; init; }
}
