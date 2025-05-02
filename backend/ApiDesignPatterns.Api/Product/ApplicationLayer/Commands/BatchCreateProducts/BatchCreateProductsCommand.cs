// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.ApplicationLayer.Commands.BatchCreateProducts;

public class BatchCreateProductsCommand
{
    public required List<DomainModels.Product> Products { get; set; }
}
