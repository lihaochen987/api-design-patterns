// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.Controllers;

public record GetSupplierResponse
{
    [Required] public long Id { get; init; }
    [Required] public required string FullName { get; init; }
    [Required] public required string Email { get; init; }
    [Required] public required string CreatedAt { get; init; }
    [Required] public required List<AddressResponse> Addresses { get; init; }
    [Required] public required List<PhoneNumberResponse> PhoneNumbers { get; init; }
}
