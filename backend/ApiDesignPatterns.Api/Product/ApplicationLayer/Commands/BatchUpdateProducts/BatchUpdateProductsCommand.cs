// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;

namespace backend.Product.ApplicationLayer.Commands.BatchUpdateProducts;

public class BatchUpdateProductsCommand
{
    public required IEnumerable<DomainModels.Product> Products { get; init; }
    public required BatchUpdateProductsRequest Request { get; init; }
}
