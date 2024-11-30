namespace backend.Product.Contracts;

public class ProductPricingContract
{
    public string BasePrice { get; init; } = "";
    public string DiscountPercentage { get; init; } = "";
    public string TaxRate { get; init; } = "";
}