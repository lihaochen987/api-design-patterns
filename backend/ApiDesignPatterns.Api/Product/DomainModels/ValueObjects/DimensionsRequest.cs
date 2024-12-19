// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.Contracts;

public class DimensionsRequest
{
    public string? Length { get; init; }
    public string? Width { get; init; }
    public string? Height { get; init; }
}
