namespace backend.Product.ProductPricingControllers;

public record GetProductPricingRequest
{
    public List<string> FieldMask { get; init; } = ["*"];
}
