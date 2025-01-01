// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;

namespace backend.Product.Services.ProductPricingServices;

public class ProductPricingFieldMaskService
{
    public Pricing
        GetUpdatedProductPricingValues(
            UpdateProductRequest request,
            Pricing product)
    {
        decimal basePrice = request.FieldMask.Contains("baseprice", StringComparer.OrdinalIgnoreCase)
                            && decimal.TryParse(request.Pricing?.BasePrice, out decimal parsedBasePrice)
            ? parsedBasePrice
            : product.BasePrice;

        decimal discountPercentage = request.FieldMask.Contains("discountpercentage", StringComparer.OrdinalIgnoreCase)
                                     && decimal.TryParse(request.Pricing?.DiscountPercentage,
                                         out decimal parsedDiscountPercentage)
            ? parsedDiscountPercentage
            : product.DiscountPercentage;

        decimal taxRate = request.FieldMask.Contains("taxrate", StringComparer.OrdinalIgnoreCase)
                          && decimal.TryParse(request.Pricing?.TaxRate, out decimal parsedTaxRate)
            ? parsedTaxRate
            : product.TaxRate;

        return new Pricing(basePrice, discountPercentage, taxRate);
    }
}
