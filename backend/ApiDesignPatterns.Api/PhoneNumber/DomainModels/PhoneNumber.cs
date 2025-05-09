// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.PhoneNumber.DomainModels.ValueObjects;

namespace backend.PhoneNumber.DomainModels;

public record PhoneNumber
{
    public required long Id { get; init; }
    public long? SupplierId { get; init; }
    public required CountryCode CountryCode { get; init; }
    public required AreaCode AreaCode { get; init; }
    public required PhoneDigits Number { get; init; }
}
