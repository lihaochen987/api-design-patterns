// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Product.Queries.GetProduct;

public record GetProductQuery : IQuery<DomainModels.Product>
{
    public required long Id { get; init; }
}
