// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.ApplicationLayer.ReplaceProduct;

public record ReplaceProductQuery
{
    public required DomainModels.Product Product { get; init; }
}
