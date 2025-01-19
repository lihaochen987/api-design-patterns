// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Commands.CreateProduct;

public record CreateProductQuery
{
    public required DomainModels.Product Product { get; init; }
}
