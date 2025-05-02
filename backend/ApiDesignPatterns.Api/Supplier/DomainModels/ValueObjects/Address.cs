// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels.ValueObjects;

public record Address
{
    public required Street Street { get; init; }
    public required City City { get; init; }
    public required PostalCode PostalCode { get; init; }
    public required Country Country { get; init; }
}
