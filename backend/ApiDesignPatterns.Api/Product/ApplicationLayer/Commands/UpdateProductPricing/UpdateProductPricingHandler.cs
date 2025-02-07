// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.ProductPricingControllers;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.UpdateProductPricing;

/// <summary>
/// Updates product pricing using field masking for selective property updates.
/// </summary>
public class UpdateProductPricingHandler(IProductRepository repository) : ICommandHandler<UpdateProductPricingCommand>
{
    /// <summary>
    /// Updates product pricing based on field mask.
    /// </summary>
    /// <example>
    /// {
    ///     "basePrice": "99.99",
    ///     "taxRate": "8.5",
    ///     "fieldMask": ["basePrice", "taxRate"]
    /// }
    /// </example>
    public async Task Handle(UpdateProductPricingCommand command)
    {
        (decimal basePrice, decimal discountPercentage, decimal taxRate) =
            GetUpdatedProductPricingValues(command.Request, command.Product.Pricing);

        var updatedProductWithPricing =
            command.Product with { Pricing = new Pricing(basePrice, discountPercentage, taxRate) };

        await repository.UpdateProductAsync(updatedProductWithPricing);
    }

    /// <summary>
    /// Returns updated pricing values based on field mask.
    /// </summary>
    private static (
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
        GetUpdatedProductPricingValues(
            UpdateProductPricingRequest request,
            Pricing product)
    {
        decimal basePrice = request.FieldMask.Contains("pricing.baseprice", StringComparer.OrdinalIgnoreCase)
                            && decimal.TryParse(request.BasePrice, out decimal parsedBasePrice)
            ? parsedBasePrice
            : product.BasePrice;

        decimal discountPercentage =
            request.FieldMask.Contains("pricing.discountpercentage", StringComparer.OrdinalIgnoreCase)
            && decimal.TryParse(request.DiscountPercentage,
                out decimal parsedDiscountPercentage)
                ? parsedDiscountPercentage
                : product.DiscountPercentage;

        decimal taxRate = request.FieldMask.Contains("pricing.taxrate", StringComparer.OrdinalIgnoreCase)
                          && decimal.TryParse(request.TaxRate, out decimal parsedTaxRate)
            ? parsedTaxRate
            : product.TaxRate;

        return (basePrice, discountPercentage, taxRate);
    }
}
