namespace backend.Product.Contracts;

public class ProductPricingResponse
{
    public required string BasePrice { get; init; }
    public required string DiscountPercentage { get; init; }
    public required string TaxRate { get; init; }
}
