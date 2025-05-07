// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Controllers;

public class UpdateSupplierRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public List<AddressRequest>? Addresses { get; init; }
    public List<PhoneNumberRequest>? PhoneNumbers { get; init; }
    public List<string> FieldMask { get; init; } = ["*"];
}
