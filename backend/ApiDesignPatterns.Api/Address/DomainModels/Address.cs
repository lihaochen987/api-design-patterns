// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.DomainModels.ValueObjects;

namespace backend.Address.DomainModels;

public record Address
{
    public required long Id { get; init; }
    public required long SupplierId { get; init; }
    public required Street Street { get; init; }
    public required City City { get; init; }
    public required PostalCode PostalCode { get; init; }
    public required Country Country { get; init; }
}
