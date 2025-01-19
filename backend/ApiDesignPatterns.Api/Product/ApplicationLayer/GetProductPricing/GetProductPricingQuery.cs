// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.GetProductPricing;

public record GetProductPricingQuery : IQuery<ProductPricingView>
{
    public required long Id { get; init; }
}
