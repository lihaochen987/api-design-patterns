namespace backend.Product.Contracts;

public class ProductPricingContract
{
    public string BasePrice { get; set; } = "";
    public string DiscountPercentage { get; set; } = "";
    public string TaxRate { get; set; } = "";
}