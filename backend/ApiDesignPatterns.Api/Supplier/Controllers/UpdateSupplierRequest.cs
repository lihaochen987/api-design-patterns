// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;

namespace backend.Supplier.Controllers;

public record UpdateSupplierRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public AddressRequest? Address { get; init; }
    public PhoneNumberRequest? PhoneNumber { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
