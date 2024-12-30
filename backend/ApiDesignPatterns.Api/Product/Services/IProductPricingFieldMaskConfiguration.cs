// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductPricingControllers;

namespace backend.Product.Services;

public interface IProductPricingFieldMaskConfiguration
{
    public (
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate)
        GetUpdatedProductPricingValues(
            UpdateProductPricingRequest request,
            Pricing product);
}
