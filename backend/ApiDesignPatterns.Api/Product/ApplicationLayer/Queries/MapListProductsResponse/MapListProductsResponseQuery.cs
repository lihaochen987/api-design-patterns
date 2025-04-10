// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ApplicationLayer.Queries.ListProducts;
using backend.Product.Controllers.Product;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.MapListProductsResponse;

public record MapListProductsResponseQuery : IQuery<ListProductsResponse>
{
    public required PagedProducts PagedProducts { get; init; }
}
