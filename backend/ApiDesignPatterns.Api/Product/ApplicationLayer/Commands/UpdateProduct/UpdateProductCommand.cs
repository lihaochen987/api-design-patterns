// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.ProductControllers;

namespace backend.Product.ApplicationLayer.Commands.UpdateProduct;

public record UpdateProductCommand
{
    public required DomainModels.Product Product { get; init; }
    public required UpdateProductRequest Request { get; init; }
}
