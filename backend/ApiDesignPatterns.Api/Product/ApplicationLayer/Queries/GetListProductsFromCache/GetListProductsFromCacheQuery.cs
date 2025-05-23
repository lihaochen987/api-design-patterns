// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;
using backend.Shared.Caching;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.GetListProductsFromCache;

public record GetListProductsFromCacheQuery : IQuery<GetListProductsFromCacheResult>
{
    public required ListProductsRequest Request { get; init; }

    public required double CheckRate { get; init; }
}
