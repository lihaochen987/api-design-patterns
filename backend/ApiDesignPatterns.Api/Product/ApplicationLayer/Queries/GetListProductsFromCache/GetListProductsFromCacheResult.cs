// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;

namespace backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;

public record GetListProductsFromCacheResult
{
    public ListProductsResponse? ProductsResponse { get; init; }
    public required string CacheKey { get; init; }
    public bool SelectedForStalenessCheck { get; init; }
}
