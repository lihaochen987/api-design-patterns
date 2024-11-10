namespace backend.Product.FieldMasks;

using DomainModels;
using ProductPricingControllers;

/// <summary>
/// ProductMaskFieldPaths is a class which holds all the manual changes required for this API to work.
/// For instance when adding a new field or value object you would have to:
/// 1. Add the new object to the AllFieldPaths (partial retrievals)
/// 2. Add parsing logic for GetUpdatedProductValues (partial updates)
/// 3. Add the mapping in the extension methods (TBC on making this more generic and easier)
/// </summary>
public class ProductPricingFieldMaskConfiguration
{
    public readonly HashSet<string> ProductPricingFieldPaths =
    [
        "*",
        "id",
        "baseprice",
        "discountpercentage",
        "taxrate"
    ];

    public (
        decimal basePrice,
        DiscountPercentage discountPercentage,
        TaxRate taxRate)
        GetUpdatedProductPricingValues(
            UpdateProductPricingRequest request,
            ProductPricing product)
    {
        var basePrice = request.FieldMask.Contains("baseprice", StringComparer.OrdinalIgnoreCase)
                        && decimal.TryParse(request.BasePrice, out var parsedBasePrice)
            ? parsedBasePrice
            : product.BasePrice;

        var discountPercentage = request.FieldMask.Contains("discountpercentage", StringComparer.OrdinalIgnoreCase)
                                 && DiscountPercentage.TryParse(request.DiscountPercentage,
                                     out var parsedDiscountPercentage)
            ? parsedDiscountPercentage!
            : product.DiscountPercentage;

        var taxRate = request.FieldMask.Contains("taxrate", StringComparer.OrdinalIgnoreCase)
                      && TaxRate.TryParse(request.TaxRate, out var parsedTaxRate)
            ? parsedTaxRate!
            : product.TaxRate;

        return (basePrice, discountPercentage, taxRate);
    }
}