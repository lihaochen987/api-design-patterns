// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.BatchGetProducts;

public record BatchGetProductsQuery : IQuery<List<Controllers.Product.GetProductResponse>>
{
    public required List<long> ProductIds { get; init; }
}
