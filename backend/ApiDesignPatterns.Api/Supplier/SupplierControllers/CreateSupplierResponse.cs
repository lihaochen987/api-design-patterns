// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.SupplierControllers;

public record CreateSupplierResponse
{
    public required string Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }
    public required string CreatedAt { get; init; }
    public required string CountryCode { get; init; }
    public required string AreaCode { get; init; }
    public required string Number { get; init; }
}
