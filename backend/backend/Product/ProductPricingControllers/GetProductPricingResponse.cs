namespace backend.Product.ProductPricingControllers;

public class GetProductPricingResponse
{
    public string Id { get; set; } = "";
    public string BasePrice { get; set; } = "";
    public string DiscountPercentage { get; set; } = "";
    public string TaxRate { get; set; } = "";
}
