// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Controllers.Product;

namespace backend.Product.ApplicationLayer.Commands.ReplaceProduct;

public record ReplaceProductCommand
{
    public required long ExistingProductId { get; init; }
    public required ReplaceProductRequest Request { get; init; }
}
