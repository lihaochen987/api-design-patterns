// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Product.DomainModels.ValueObjects;

public record WeightKgRequest
{
    public string? Value { get; init; }
}
