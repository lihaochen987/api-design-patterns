// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

public record PhoneNumber
{
    public required CountryCode CountryCode { get; init; }
    public required AreaCode AreaCode { get; init; }
    public required PhoneDigits Number { get; init; }
}
