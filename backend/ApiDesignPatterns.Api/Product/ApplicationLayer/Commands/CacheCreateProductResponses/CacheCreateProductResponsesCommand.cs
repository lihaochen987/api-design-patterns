// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;

namespace backend.Product.ApplicationLayer.Commands.CacheCreateProductResponses;

public class CacheCreateProductResponsesCommand
{
    public required string RequestId { get; init; }
    public required IEnumerable<CreateProductRequest> CreateProductRequests { get; init; }
    public required IEnumerable<CreateProductResponse> CreateProductResponses { get; init; }
}
