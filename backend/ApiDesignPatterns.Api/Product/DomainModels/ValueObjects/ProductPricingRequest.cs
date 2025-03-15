// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

public class ProductPricingRequest
{
    public decimal? BasePrice { get; init; }
    public decimal? DiscountPercentage { get; init; }
    public decimal? TaxRate { get; init; }
}
