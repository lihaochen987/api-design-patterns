// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

public record PhoneNumber
{
    public required string CountryCode { get; init; }
    public required string AreaCode { get; init; }
    public long Number { get; init; }
}
