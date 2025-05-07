// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.DomainModels;

public record SupplierView : Identifier
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public required List<Address> Addresses { get; init; }
    public required List<PhoneNumber> PhoneNumbers { get; init; }
}
