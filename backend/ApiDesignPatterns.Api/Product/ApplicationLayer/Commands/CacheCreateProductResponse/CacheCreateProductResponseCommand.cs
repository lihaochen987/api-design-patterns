// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;

namespace backend.Product.ApplicationLayer.Commands.CacheCreateProductResponse;

public record CacheCreateProductResponseCommand
{
    public required string RequestId { get; init; }
    public required CreateProductRequest CreateProductRequest { get; init; }
    public required CreateProductResponse CreateProductResponse { get; init; }
}
