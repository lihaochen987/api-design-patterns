// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.ProductPricingControllers;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.UpdateProductPricing;

/// <summary>
/// Handles commands to update product pricing information in the system.
/// </summary>
/// <remarks>
/// This handler processes partial updates to product pricing, allowing individual price components
/// (base price, discount percentage, and tax rate) to be updated independently based on the provided field mask.
/// </remarks>
public class UpdateProductPricingHandler(IProductRepository repository) : ICommandHandler<UpdateProductPricingQuery>
{
    /// <summary>
    /// Processes the update product pricing command.
    /// </summary>
    /// <param name="command">The command containing the product and pricing update request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// var handler = new UpdateProductPricingHandler(repository);
    /// var command = new UpdateProductPricingQuery
    /// {
    ///     Product = existingProduct,
    ///     Request = new UpdateProductPricingRequest
    ///     {
    ///         BasePrice = "99.99",
    ///         DiscountPercentage = "10.0",
    ///         TaxRate = "8.5",
    ///         FieldMask = new[] { "basePrice", "taxRate" }
    ///     }
    /// };
    /// await handler.Handle(command);
    /// </code>
    /// </example>
    public async Task Handle(UpdateProductPricingQuery command)
    {
        (decimal basePrice, decimal discountPercentage, decimal taxRate) =
            GetUpdatedProductPricingValues(command.Request, command.Product.Pricing);

        command.Product.Pricing = new Pricing(basePrice, discountPercentage, taxRate);

        await repository.UpdateProductAsync(command.Product);
    }

    /// <summary>
    /// Determines which pricing values should be updated based on the field mask and parses the new values.
    /// </summary>
    /// <param name="request">The update request containing new pricing values and field mask.</param>
    /// <param name="product">The current product pricing information.</param>
    /// <returns>A tuple containing the updated or existing values for (basePrice, discountPercentage, taxRate).</returns>
    /// <remarks>
    /// For each pricing component (base price, discount percentage, tax rate):
    /// - If the field is included in the field mask and can be parsed, the new value is used
    /// - Otherwise, the existing value from the product is retained
    /// </remarks>
    /// <example>
    /// Given request:
    /// <code>
    /// var request = new UpdateProductPricingRequest
    /// {
    ///     BasePrice = "199.99",
    ///     DiscountPercentage = "15.0",
    ///     TaxRate = "7.5",
    ///     FieldMask = new[] { "basePrice", "taxRate" }
    /// };
    ///
    /// var currentPricing = new Pricing(
    ///     basePrice: 149.99m,
    ///     discountPercentage: 10.0m,
    ///     taxRate: 8.5m
    /// );
    ///
    /// // Result will be:
    /// // basePrice: 199.99 (updated from request)
    /// // discountPercentage: 10.0 (retained from current)
    /// // taxRate: 7.5 (updated from request)
    /// var result = GetUpdatedProductPricingValues(request, currentPricing);
    /// </code>
    /// </example>
    private (
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
