// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;
using backend.Shared.Caching;
using MassTransit.Caching.Internals;

namespace backend.Product.ApplicationLayer.Commands.UpdateListProductsStaleness;

public record UpdateListProductStalenessCommand
{
    public required ListProductsResponse FreshResult { get; init; }
    public required ListProductsResponse CachedResult{ get; init; }
    public required CacheStalenessOptions StalenessOptions { get; init; }
}
