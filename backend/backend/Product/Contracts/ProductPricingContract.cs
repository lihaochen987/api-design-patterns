using System.ComponentModel.DataAnnotations;

namespace backend.Product.Contracts;

public class ProductPricingContract
{
    [Required] public string BasePrice { get; init; } = "";
    [Required] public string DiscountPercentage { get; init; } = "";
    [Required] public string TaxRate { get; init; } = "";
}
