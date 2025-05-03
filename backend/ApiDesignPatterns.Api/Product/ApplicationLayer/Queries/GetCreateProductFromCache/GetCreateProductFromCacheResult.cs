// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;

namespace backend.Product.ApplicationLayer.Queries.GetCreateProductFromCache;

public class GetCreateProductFromCacheResult
{
    public CreateProductResponse? CreateProductResponse { get; init; }
    public string? Hash { get; init; }
}
