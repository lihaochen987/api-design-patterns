// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.ProductPricingControllers;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.UpdateProductPricing;

public class UpdateProductPricingHandler(IProductRepository repository) : ICommandHandler<UpdateProductPricingQuery>
{
    public async Task Handle(UpdateProductPricingQuery command)
    {
        (decimal basePrice, decimal discountPercentage, decimal taxRate) =
            GetUpdatedProductPricingValues(command.Request, command.Product.Pricing);

        command.Product.Pricing = new Pricing(basePrice, discountPercentage, taxRate);

        await repository.UpdateProductAsync(command.Product);
    }

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
