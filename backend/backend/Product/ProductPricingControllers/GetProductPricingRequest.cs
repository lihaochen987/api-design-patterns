namespace backend.Product.ProductPricingControllers;

public class GetProductPricingRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}