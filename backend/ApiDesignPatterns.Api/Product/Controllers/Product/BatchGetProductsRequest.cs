// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Controllers.Product;

public class BatchGetProductsRequest
{
    public List<long> Ids { get; set; } = [];
}
