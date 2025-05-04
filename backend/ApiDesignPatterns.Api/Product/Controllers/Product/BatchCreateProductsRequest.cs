// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Controllers.Product;

public class BatchCreateProductsRequest
{
    public string? RequestId { get; init; }
    public IEnumerable<CreateProductRequest> Products { get; set; } = [];
}
