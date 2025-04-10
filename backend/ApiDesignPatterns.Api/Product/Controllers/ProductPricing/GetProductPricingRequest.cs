namespace backend.Product.Controllers.ProductPricing;

public record GetProductPricingRequest
{
    public List<string> FieldMask { get; init; } = ["*"];
}
