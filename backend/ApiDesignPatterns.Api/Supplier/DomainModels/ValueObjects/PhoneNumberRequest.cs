// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

public record PhoneNumberRequest
{
    public string? CountryCode { get; init; }
    public string? AreaCode { get; init; }
    public string? Number { get; init; }
}
