// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Product.ApplicationLayer.Queries.CreateProductResponse;

public record CreateProductResponseQuery : IQuery<ProductControllers.CreateProductResponse>
{
    public required DomainModels.Product Product { get; init; }
}
