// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;

public record CacheQueryResult
{
    public ListProductsResponse? ProductsResponse { get; init; }
    public required string cacheKey { get; init; }
}
