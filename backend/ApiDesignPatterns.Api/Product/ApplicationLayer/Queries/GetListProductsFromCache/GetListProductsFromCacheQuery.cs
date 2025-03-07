// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;

public record GetListProductsFromCacheQuery : IQuery<ListProductsResponse>
{
    public required string CacheKey { get; init; }
}
