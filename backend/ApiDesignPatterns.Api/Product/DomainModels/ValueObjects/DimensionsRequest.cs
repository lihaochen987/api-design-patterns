// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

public class DimensionsRequest
{
    public decimal? Length { get; init; }
    public decimal? Width { get; init; }
    public decimal? Height { get; init; }
}
