// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer.Commands.PersistListProductsToCache;

public record PersistListProductsToCacheCommand
{
    public required string CacheKey { get; init; }
    public required ListProductsResponse Products { get; init; }
    public TimeSpan? Expiry { get; init; }
}
