// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.SupplierControllers;

public record GetSupplierResponse
{
    public long Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string CreatedAt { get; init; }
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }
    public required string PhoneNumber { get; init; }
}
