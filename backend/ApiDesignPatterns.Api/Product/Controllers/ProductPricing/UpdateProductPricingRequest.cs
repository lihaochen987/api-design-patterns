namespace backend.Product.Controllers.ProductPricing;

public record UpdateProductPricingRequest
{
    public string? BasePrice { get; init; }
    public string? DiscountPercentage { get; init; }
    public string? TaxRate { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
