// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Contracts;

public class ProductPricingRequest
{
    public string? BasePrice { get; init; }
    public string? DiscountPercentage { get; init; }
    public string? TaxRate { get; init; }
}
