// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.ListProducts;

public record ListProductsQuery : IQuery<(List<ProductView>, string?)>
{
    public string? Filter { get; init; }
    public string? PageToken { get; init; }
    public required int MaxPageSize { get; init; }
}
