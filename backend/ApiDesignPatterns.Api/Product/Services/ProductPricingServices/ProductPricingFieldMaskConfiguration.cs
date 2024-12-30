using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductPricingControllers;

namespace backend.Product.Services.ProductPricingServices;

/// <summary>
///     ProductMaskFieldPaths is a class which holds all the manual changes required for this API to work.
///     For instance when adding a new field or value object you would have to:
///     1. Add the new object to the AllFieldPaths (partial retrievals)
///     2. Add parsing logic for GetUpdatedProductValues (partial updates)
///     3. Add the mapping in the extension methods (TBC on making this more generic and easier)
/// </summary>
public class ProductPricingFieldMaskConfiguration : IProductPricingFieldMaskConfiguration
{
    public (
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
        GetUpdatedProductPricingValues(
            UpdateProductPricingRequest request,
            Pricing product)
    {
        decimal basePrice = request.FieldMask.Contains("baseprice", StringComparer.OrdinalIgnoreCase)
                            && decimal.TryParse(request.BasePrice, out decimal parsedBasePrice)
            ? parsedBasePrice
            : product.BasePrice;

        decimal discountPercentage = request.FieldMask.Contains("discountpercentage", StringComparer.OrdinalIgnoreCase)
                                     && decimal.TryParse(request.DiscountPercentage,
                                         out decimal parsedDiscountPercentage)
            ? parsedDiscountPercentage
            : product.DiscountPercentage;

        decimal taxRate = request.FieldMask.Contains("taxrate", StringComparer.OrdinalIgnoreCase)
                          && decimal.TryParse(request.TaxRate, out decimal parsedTaxRate)
            ? parsedTaxRate
            : product.TaxRate;

        return (basePrice, discountPercentage, taxRate);
    }
}
