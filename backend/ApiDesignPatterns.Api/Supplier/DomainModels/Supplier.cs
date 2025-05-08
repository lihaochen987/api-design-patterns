// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.DomainModels;

public record Supplier
{
    public long Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required ICollection<Address> Addresses { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public required ICollection<ValueObjects.PhoneNumber> PhoneNumbers { get; init; }
}
