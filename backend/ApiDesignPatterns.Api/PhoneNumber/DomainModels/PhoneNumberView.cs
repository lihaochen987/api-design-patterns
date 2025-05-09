// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.PhoneNumber.DomainModels;

public record PhoneNumberView
{
    public required long Id { get; init; }
    public required long SupplierId { get; init; }
    public required string PhoneNumber { get; init; }
}
