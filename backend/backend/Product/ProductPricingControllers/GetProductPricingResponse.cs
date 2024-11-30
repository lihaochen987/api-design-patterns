using System.ComponentModel.DataAnnotations;

namespace backend.Product.ProductPricingControllers;

public class GetProductPricingResponse
{
    [Required] public string Id { get; set; } = "";
    [Required] public string BasePrice { get; set; } = "";
    [Required] public string DiscountPercentage { get; set; } = "";
    [Required] public string TaxRate { get; set; } = "";
}
