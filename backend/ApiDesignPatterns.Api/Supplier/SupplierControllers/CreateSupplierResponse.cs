// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;

namespace backend.Supplier.SupplierControllers;

public record CreateSupplierResponse
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required AddressResponse Address { get; init; }
    public required string CreatedAt { get; init; }
    public required PhoneNumberResponse PhoneNumber { get; init; }
}
