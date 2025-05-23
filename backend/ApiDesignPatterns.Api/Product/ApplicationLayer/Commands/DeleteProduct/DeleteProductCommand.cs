// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.ApplicationLayer.Commands.DeleteProduct;

public record DeleteProductCommand
{
    public required long Id { get; init; }
}
