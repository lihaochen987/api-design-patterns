// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Controllers.Product;

public record UpdateProductRequestWithId : UpdateProductRequest
{
    public long Id { get; init; }
}

public class BatchUpdateProductsRequest
{
    public IEnumerable<UpdateProductRequestWithId>? Products { get; set; }
}
