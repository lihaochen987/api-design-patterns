namespace backend.Product.ProductPricingControllers;

public record GetProductPricingResponse
{
    public required string Id { get; init; }
    public required string BasePrice { get; init; }
    public required string DiscountPercentage { get; init; }
    public required string TaxRate { get; init; }
}
