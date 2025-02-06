// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductPricingControllers;

namespace backend.Product.ApplicationLayer.Commands.UpdateProductPricing;

public record UpdateProductPricingCommand
{
    public required DomainModels.Product Product { get; init; }
    public required UpdateProductPricingRequest Request { get; init; }
}
