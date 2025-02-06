// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.ApplicationLayer.Commands.CreateProduct;

public record CreateProductCommand
{
    public required DomainModels.Product Product { get; init; }
}
